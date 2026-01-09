using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Reserveit.Application.CurrentUserService;
using Reserveit.Application.Interfaces;
using Reserveit.Domain.Enums;
using Reserveit.Domain.Exceptions;
using Reserveit.Domain.Interfaces;

namespace Reserveit.Application.CommandsQueriesForModels.Staffes.Commands.ChangeReservationStatus;

public sealed class ChangeStaffReservationStatusCommandHandler
    : IRequestHandler<ChangeStaffReservationStatusCommand>
{
    private readonly ICurrentUser _currentUser;
    private readonly IStaffRepository _staffRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IValidator<ChangeStaffReservationStatusCommand> _validator;
    private readonly ILogger<ChangeStaffReservationStatusCommandHandler> _logger;
    private readonly INotificationQueue  _notificationQueue;

    public ChangeStaffReservationStatusCommandHandler(
        ICurrentUser currentUser,
        IStaffRepository staffRepository,
        IReservationRepository reservationRepository,
        IValidator<ChangeStaffReservationStatusCommand> validator,
        ILogger<ChangeStaffReservationStatusCommandHandler> logger,
        INotificationQueue notificationQueue)
    {
        _currentUser = currentUser;
        _staffRepository = staffRepository;
        _reservationRepository = reservationRepository;
        _validator = validator;
        _logger = logger;
        _notificationQueue = notificationQueue;
    }

    public async Task Handle(ChangeStaffReservationStatusCommand request, CancellationToken ct)
    {
        var vr = await _validator.ValidateAsync(request, ct);
        if (!vr.IsValid)
            throw new ValidationException(vr.Errors);

        var userId = _currentUser.UserId;

        var staff = await _staffRepository.GetByUserIdAsync(userId, ct);
        if (staff is null)
        {
            _logger.LogWarning("ChangeStatus: staff profile not found. UserId={UserId}", userId);
            throw new UnauthorizedAccessException("Staff profile not found.");
        }

        var reservation = await _reservationRepository.GetByIdAsync(request.ReservationId, ct);
        if (reservation is null)
        {
            _logger.LogWarning("ChangeStatus: reservation not found. ReservationId={ReservationId}", request.ReservationId);
            throw new InvalidOperationException("Reservation not found.");
        }

        if (reservation.StaffId != staff.Id)
        {
            _logger.LogWarning(
                "ChangeStatus forbidden: staff mismatch. ReservationId={ReservationId}, ReservationStaffId={ReservationStaffId}, StaffId={StaffId}",
                reservation.Id, reservation.StaffId, staff.Id);

            throw new UnauthorizedAccessException("User don't have access to this reservation.");
        }

        var from = reservation.Status;
        var to = request.Data.Status;

        // you can skip changing to the same status
        if (from == to)
            return;

        // Фінальні стани не чіпаємо
        if (from is ReservationStatus.Cancelled or ReservationStatus.Completed)
            throw new InvalidOperationException("Changing status from Completed | Cancelled is forbidden.");

        // allowed changes for staff:
        // Pending -> Confirmed | Cancelled
        // Confirmed -> Completed | Cancelled
        var allowed = from switch
        {
            ReservationStatus.Pending => to is ReservationStatus.Confirmed or ReservationStatus.Cancelled,
            ReservationStatus.Confirmed => to is ReservationStatus.Completed or ReservationStatus.Cancelled,
            _ => false
        };

        if (!allowed)
            throw new ForbiddenException($"Forbidden status switch: {from} -> {to}.");

        // Completed тільки якщо вже пройшов час завершення
        if (to == ReservationStatus.Completed && reservation.EndAt > DateTimeOffset.UtcNow)
            throw new InvalidOperationException("You are not allowed to comlete reservation until it ends.");

        reservation.Status = to;
        reservation.UpdatedAt = DateTimeOffset.UtcNow;

        await _reservationRepository.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Reservation status changed by staff. ReservationId={ReservationId}, From={From}, To={To}, StaffId={StaffId}",
            reservation.Id, from, to, staff.Id);


        await _notificationQueue.EnqueueReservationStatusChangedAsync(reservation.Id, from.ToString(), to.ToString(), ct);
    }
}
