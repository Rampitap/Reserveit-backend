using Reserveit.Application.Common.DTOs.ServiceDtos;
using Reserveit.Application.Common.DTOs.StaffDtos;

namespace Reserveit.Application.Common.DTOs.BuisnessDtos;

public sealed class PublicBusinessDetailsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Address { get; set; }
    public string Timezone { get; set; } = "UTC";
    public string? OpeningTime { get; set; }
    public string? ClosingTime { get; set; }
    public string? ImageUrl { get; set; }
    public Guid? CategoryId { get; set; }
    public string? CategoryName { get; init; }
    public List<PublicServiceDto> Services { get; set; } = new();
    public List<PublicStaffDto> StaffMembers { get; set; } = new();
}

