namespace Reserveit.Application.Common.DTOs.ServiceDtos;

public record CreateServiceDto
(
    string Name,
    string? Description,
    decimal? Price,
    int DurationMinutes
);
