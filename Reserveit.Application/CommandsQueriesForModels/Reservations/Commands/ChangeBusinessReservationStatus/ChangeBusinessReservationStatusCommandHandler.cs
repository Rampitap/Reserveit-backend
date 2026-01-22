using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Reserveit.Application.CurrentUserService;
using Reserveit.Application.Interfaces;
using Reserveit.Domain.Entities;
using Reserveit.Domain.Enums;
using Reserveit.Domain.Exceptions;
using Reserveit.Domain.Interfaces;

namespace Reserveit.Application.CommandsQueriesForModels.Reservations.Commands.ChangeBusinessReservationStatus;

public sealed class ChangeBusinessReservationStatusCommandHandler
    : IRequestHandler<ChangeBusinessReservationStatusCommand>
{
    private readonly ICurrentUser _currentUser;
    private readonly IBusinessRepository _businessRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IValidator<ChangeBusinessReservationStatusCommand> _validator;
    private readonly ILogger<ChangeBusinessReservationStatusCommandHandler> _logger;
    private readonly INotificationQueue _notificationQueue;

    public ChangeBusinessReservationStatusCommandHandler(
        ICurrentUser currentUser,
        IBusinessRepository businessRepository,
        IReservationRepository reservationRepository,
        IValidator<ChangeBusinessReservationStatusCommand> validator,
        ILogger<ChangeBusinessReservationStatusCommandHandler> logger,
        INotificationQueue notificationQueue)
    {
        _currentUser = currentUser;
        _businessRepository = businessRepository;
        _reservationRepository = reservationRepository;
        _validator = validator;
        _logger = logger;
        _notificationQueue = notificationQueue;
    }

    public async Task Handle(ChangeBusinessReservationStatusCommand request, CancellationToken ct)
    {
        var vr = await _validator.ValidateAsync(request, ct);
        if (!vr.IsValid) throw new ValidationException(vr.Errors);

        var ownerId = _currentUser.UserId;

        // 1) owner must own businessId from route
        var business = await _businessRepository.GetByIdAsync(request.BusinessId, ct)
            ?? throw new NotFoundException(nameof(Business), request.BusinessId.ToString());

        if (business.OwnerId != ownerId)
        {
            _logger.LogWarning("Owner access denied. OwnerId={OwnerId}, BusinessId={BusinessId}", ownerId, business.Id);
            throw new ForbiddenException("You don't have access to this business.");
        }

        // 2) reservation must exist and belong to this business
        var reservation = await _reservationRepository.GetByIdAsync(request.ReservationId, ct)
            ?? throw new NotFoundException(nameof(Reservation), request.ReservationId.ToString());

        if (reservation.BusinessId != business.Id)
        {
            _logger.LogWarning("Owner change status denied: reservation not in business. ReservationId={ReservationId}, BusinessId={BusinessId}",
                reservation.Id, business.Id);
            throw new ForbiddenException("Reservation doesn't belong to this business.");
        }

        var from = reservation.Status;
        var to = request.Data.Status;

        if (from == to) return;

        // 3) final states are locked
        if (from is ReservationStatus.Cancelled or ReservationStatus.Completed)
            throw new InvalidOperationException("Changing status from Completed | Cancelled is forbidden.");

        // 4) allowed transitions (same rules as staff to keep it safe)
        var allowed = from switch
        {
            ReservationStatus.Pending => to is ReservationStatus.Confirmed or ReservationStatus.Cancelled,
            ReservationStatus.Confirmed => to is ReservationStatus.Completed or ReservationStatus.Cancelled,
            _ => false
        };

        if (!allowed)
            throw new ForbiddenException($"Forbidden status switch: {from} -> {to}.");

        if (to == ReservationStatus.Completed && reservation.EndAt > DateTimeOffset.UtcNow)
            throw new InvalidOperationException("You can't complete reservation until it ends.");

        reservation.Status = to;
        reservation.UpdatedAt = DateTimeOffset.UtcNow;

        await _reservationRepository.SaveChangesAsync(ct);

        _logger.LogInformation("Owner changed reservation status. ReservationId={ReservationId}, From={From}, To={To}, BusinessId={BusinessId}, OwnerId={OwnerId}",
            reservation.Id, from, to, business.Id, ownerId);

        await _notificationQueue.EnqueueReservationStatusChangedAsync(
            reservation.Id, from.ToString(), to.ToString(), ct);
    }
}
