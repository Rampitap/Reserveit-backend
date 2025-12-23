namespace Reserveit.Application.Common.DTOs.BuisnessDtos;

public record CreateBusinessDto
(
    string Name,
    string? Description,
    string? Address,
    string Timezone,
    string? OpeningTime,
    string? ClosingTime
);
