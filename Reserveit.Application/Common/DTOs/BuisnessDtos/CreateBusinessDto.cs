namespace Reserveit.Application.Common.DTOs.BuisnessDtos;

public sealed class CreateBusinessDto
{
    public string Name { get; init; } = null!;
    public string? Address { get; init; }
    public string Timezone { get; init; } = "UTC";
    public TimeSpan? OpeningTime { get; init; }
    public TimeSpan? ClosingTime { get; init; }
    public string? CancellationPolicyJson { get; init; }
    public Guid? CategoryId { get; init; }
    public string? ImageUrl { get; init; }
    //public Guid? CategoryId { get; init; }
}