using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Reserveit.Application.CurrentUserService;
using Reserveit.Application.Interfaces;
using Reserveit.Domain.Enums;
using Reserveit.Domain.Exceptions;
using Reserveit.Domain.Interfaces;

namespace Reserveit.Application.CommandsQueriesForModels.Clients.Commands.CancelClientReservation;

public sealed class CancelClientReservationCommandHandler : IRequestHandler<CancelClientReservationCommand>
{
    private readonly ICurrentUser _currentUser;
    private readonly IReservationRepository _reservationRepository;
    private readonly IValidator<CancelClientReservationCommand> _validator;
    private readonly ILogger<CancelClientReservationCommandHandler> _logger;
    private readonly INotificationQueue _notificationQueue;

    public CancelClientReservationCommandHandler(
        ICurrentUser currentUser,
        IReservationRepository reservationRepository,
        IValidator<CancelClientReservationCommand> validator,
        ILogger<CancelClientReservationCommandHandler> logger,
        INotificationQueue notificationQueue)
    {
        _currentUser = currentUser;
        _reservationRepository = reservationRepository;
        _validator = validator;
        _logger = logger;
        _notificationQueue = notificationQueue;
    }

    public async Task Handle(CancelClientReservationCommand request, CancellationToken ct)
    {
        var vr = await _validator.ValidateAsync(request, ct);
        if (!vr.IsValid)
            throw new ValidationException(vr.Errors);

        var reservation = await _reservationRepository.GetByIdAsync(request.ReservationId, ct)
            ?? throw new NotFoundException("Reservation", request.ReservationId.ToString());

        // ownership
        if (reservation.ClientId != _currentUser.UserId)
            throw new ForbiddenException("Mistake happened, you can't cancel someonelse's reservation");

        // only active statuses can be cancelled
        if (reservation.Status is not (ReservationStatus.Pending or ReservationStatus.Confirmed))
            throw new InvalidOperationException("Anable to cancel this reservation (inactive status).");

        // cannot cancel past reservations
        if (reservation.StartAt <= DateTimeOffset.UtcNow)
            throw new InvalidOperationException("Anable to cancel this reservation (past reservations).");

        var from = reservation.Status;                
        var to = ReservationStatus.Cancelled;

        reservation.Status = ReservationStatus.Cancelled;
        reservation.UpdatedAt = DateTimeOffset.UtcNow;

        await _reservationRepository.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Client cancelled reservation. ReservationId={ReservationId}, ClientId={ClientId}",
            reservation.Id, _currentUser.UserId);

        await _notificationQueue.EnqueueReservationStatusChangedAsync(
        reservation.Id,
        from.ToString(),
        to.ToString(),
        ct);
    }
}
