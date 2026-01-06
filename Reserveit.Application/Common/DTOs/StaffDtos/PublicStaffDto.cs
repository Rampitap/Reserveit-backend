using Reserveit.Application.Common.DTOs.ServiceDtos;

namespace Reserveit.Application.Common.DTOs.StaffDtos;

public sealed class PublicStaffDto
{
    public Guid Id { get; set; }
    public string DisplayName { get; set; } = null!;
    public string? Bio { get; set; }

    public List<PublicServiceMiniDto> Services { get; set; } = new();
}
