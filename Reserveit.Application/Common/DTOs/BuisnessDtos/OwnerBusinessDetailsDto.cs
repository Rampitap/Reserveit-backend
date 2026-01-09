namespace Reserveit.Application.Common.DTOs.BuisnessDtos;

public sealed class OwnerBusinessDetailsDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = "";
    public string? Address { get; init; }
    public string Timezone { get; init; } = "UTC";
    public TimeSpan? OpeningTime { get; init; }
    public TimeSpan? ClosingTime { get; init; }
    public string? CancellationPolicyJson { get; init; }
    public bool IsActive { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }
}
