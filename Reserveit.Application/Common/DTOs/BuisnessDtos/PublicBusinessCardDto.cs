namespace Reserveit.Application.Common.DTOs.BuisnessDtos;

public sealed class PublicBusinessCardDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = "";
    public string? Address { get; init; }

    public Guid? CategoryId { get; init; }
    public string? CategoryName { get; init; }
}
