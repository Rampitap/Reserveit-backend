namespace Reserveit.Application.Common.DTOs.ServiceDtos;

public record ServiceDto
(
    Guid Id,
    Guid BusinessId, 
    string Name,
    string? Description,
    decimal? Price,
    int DurationMinutes
);
