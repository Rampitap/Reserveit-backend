using Reserveit.Application.Common.DTOs.ServiceDtos;
using Reserveit.Application.Common.DTOs.StaffDtos;

namespace Reserveit.Application.Common.DTOs.BuisnessDtos;

public record BusinessDetailsDto
(
    Guid Id,
    Guid OwnerId,       
    string Name,
    string? Description,
    string? Address,
    string Timezone,
    string? OpeningTime,
    string? ClosingTime,

    
    List<ServiceDto> Services,
    List<StaffDto> StaffMembers
);
