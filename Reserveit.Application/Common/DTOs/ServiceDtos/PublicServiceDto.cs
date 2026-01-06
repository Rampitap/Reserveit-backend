using Reserveit.Application.Common.DTOs.StaffDtos;

namespace Reserveit.Application.Common.DTOs.ServiceDtos;

public sealed class PublicServiceDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int DurationMinutes { get; set; }
    public decimal? Price { get; set; }

    public List<PublicStaffMiniDto> Staff { get; set; } = new();
}