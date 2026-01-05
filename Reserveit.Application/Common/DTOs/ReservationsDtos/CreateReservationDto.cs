namespace Reserveit.Application.Common.DTOs.ReservationsDtos;

public record CreateReservationDto
(
    Guid BusinessId,     
    Guid ServiceId,
    Guid StaffId,        
    DateTimeOffset StartAt,
    string? Notes
);
