namespace Reserveit.Application.Common.DTOs.BuisnessDtos;

public sealed class PublicBusinessSummaryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Address { get; set; }
    public string Timezone { get; set; } = "UTC";
    public string? OpeningTime { get; set; }
    public string? ClosingTime { get; set; }
}