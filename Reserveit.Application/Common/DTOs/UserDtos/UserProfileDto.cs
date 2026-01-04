namespace Reserveit.Application.Common.DTOs.UserDtos;

public sealed class UserProfileDto
{
    public Guid Id { get; init; }
    public string Email { get; init; } = default!;
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public Guid? BusinessId { get; init; }
    public IReadOnlyList<string> Roles { get; set; } = Array.Empty<string>();
}
