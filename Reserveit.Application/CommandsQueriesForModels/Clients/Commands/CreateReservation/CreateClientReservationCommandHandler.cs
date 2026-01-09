using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Reserveit.Application.CurrentUserService;
using Reserveit.Application.Interfaces;
using Reserveit.Domain.Entities;
using Reserveit.Domain.Enums;
using Reserveit.Domain.Exceptions;
using Reserveit.Domain.Interfaces;

namespace Reserveit.Application.CommandsQueriesForModels.Clients.Commands.CreateReservation;

public sealed class CreateClientReservationCommandHandler
    : IRequestHandler<CreateClientReservationCommand, Guid>
{
    private readonly ICurrentUser _currentUser;
    private readonly IServiceRepository _serviceRepository;
    private readonly IStaffRepository _staffRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IValidator<CreateClientReservationCommand> _validator;
    private readonly ILogger<CreateClientReservationCommandHandler> _logger;
    private readonly INotificationQueue _notificationQueue;

    public CreateClientReservationCommandHandler(
        ICurrentUser currentUser,
        IServiceRepository serviceRepository,
        IStaffRepository staffRepository,
        IReservationRepository reservationRepository,
        IValidator<CreateClientReservationCommand> validator,
        ILogger<CreateClientReservationCommandHandler> logger,
        INotificationQueue notificationQueue)
    {
        _currentUser = currentUser;
        _serviceRepository = serviceRepository;
        _staffRepository = staffRepository;
        _reservationRepository = reservationRepository;
        _validator = validator;
        _logger = logger;
        _notificationQueue = notificationQueue;
    }

    public async Task<Guid> Handle(CreateClientReservationCommand request, CancellationToken ct)
    {
        var vr = await _validator.ValidateAsync(request, ct);
        if (!vr.IsValid)
            throw new ValidationException(vr.Errors);

        var dto = request.Data;
        var clientId = _currentUser.UserId;

        // Service
        var service = await _serviceRepository.GetByIdAsync(dto.ServiceId, ct)
            ?? throw new NotFoundException(nameof(Service), dto.ServiceId.ToString());

        if (service.BusinessId != dto.BusinessId)
            throw new InvalidOperationException("Service dosen't belong to this business.");

        // Staff
        var staff = await _staffRepository.GetByIdAsync(dto.StaffId, ct)
            ?? throw new NotFoundException("Staff", dto.StaffId.ToString());

        if (staff.BusinessId != dto.BusinessId)
            throw new InvalidOperationException("Stuff doesn't belong to rhis business.");

        // EndAt
        var endAt = dto.StartAt.AddMinutes(service.DurationMinutes);

        // Slot check
        var slotOk = await _reservationRepository.IsSlotAvailableAsync(
            dto.BusinessId, dto.StaffId, dto.StartAt, endAt, ct);

        if (!slotOk)
            throw new InvalidOperationException("please select different time because all appointments are scheduled in this slot");

        // Create reservation
        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            BusinessId = dto.BusinessId,
            ServiceId = dto.ServiceId,
            ClientId = clientId,
            StaffId = dto.StaffId,
            StartAt = dto.StartAt,
            EndAt = endAt,
            Status = ReservationStatus.Pending,
            Notes = dto.Notes,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        await _reservationRepository.AddAsync(reservation, ct);
        await _reservationRepository.SaveChangesAsync(ct);

        _logger.LogInformation("Client reservation created. Id={Id}", reservation.Id);
        await _notificationQueue.EnqueueReservationCreatedAsync(reservation.Id, ct);

        return reservation.Id;
    }
}
