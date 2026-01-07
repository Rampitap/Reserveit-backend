namespace Reserveit.Application.Common.DTOs.StaffDtos;

public sealed class CreateStaffAccountDto
{
    public Guid BusinessId { get; set; }

    // Identity User
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    // Staff entity
    public string DisplayName { get; set; } = null!;
    public string? Bio { get; set; }
}
