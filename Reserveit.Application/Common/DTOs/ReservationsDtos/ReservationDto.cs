namespace Reserveit.Application.Common.DTOs.ReservationsDtos;

public record ReservationDto
(
    Guid Id,

    
    Guid BusinessId,
    string BusinessName,
    string? BusinessAddress,

    
    Guid ServiceId,
    string ServiceName,
    decimal? Price,
    int DurationMinutes,

    
    Guid? StaffId,
    string? StaffName,

    
    DateTimeOffset StartAt,
    DateTimeOffset EndAt,
    string Status,      
    string? Notes
);
