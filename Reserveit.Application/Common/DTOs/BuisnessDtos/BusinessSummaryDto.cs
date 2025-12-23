namespace Reserveit.Application.Common.DTOs.BuisnessDtos;

public record BusinessSummaryDto
(
     Guid Id,
    string Name,
    string? Address,
    string? OpeningTime,
    string? ClosingTime
);

