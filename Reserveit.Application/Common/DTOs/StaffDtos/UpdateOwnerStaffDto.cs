namespace Reserveit.Application.Common.DTOs.StaffDtos;

public sealed class UpdateOwnerStaffDto
{
    public string DisplayName { get; init; } = null!;
    public string? Bio { get; init; }
}
