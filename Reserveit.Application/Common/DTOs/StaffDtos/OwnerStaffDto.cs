namespace Reserveit.Application.Common.DTOs.StaffDtos;

public sealed class OwnerStaffDto
{
    public Guid Id { get; set; }
    public Guid BusinessId { get; set; }

    public Guid UserId { get; set; }          
    public string DisplayName { get; set; } = null!;
    public string? Bio { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; }
}