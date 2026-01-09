namespace Reserveit.Application.Common.DTOs.AdminManageDtos;

public sealed class AdminUserDto
{
    public Guid Id { get; init; }
    public string Email { get; init; } = "";
    public string FirstName { get; init; } = "";
    public string LastName { get; init; } = "";
    public bool IsActive { get; init; }

    public Guid? BusinessId { get; init; }  

    public IReadOnlyList<string> Roles { get; init; } = Array.Empty<string>();
    public DateTimeOffset CreatedAt { get; init; }
}
