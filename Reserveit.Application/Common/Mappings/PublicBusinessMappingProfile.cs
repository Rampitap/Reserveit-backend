using AutoMapper;
using Reserveit.Application.Common.DTOs.BuisnessDtos;
using Reserveit.Application.Common.DTOs.ServiceDtos;
using Reserveit.Application.Common.DTOs.StaffDtos;
using Reserveit.Domain.Entities;

namespace Reserveit.Application.Common.Mappings;

public sealed class PublicBusinessMappingProfile : Profile
{
    public PublicBusinessMappingProfile()
    {
        // Business -> PublicBusinessDetailsDto
        CreateMap<Business, PublicBusinessDetailsDto>()
            .ForMember(d => d.OpeningTime, o => o.MapFrom(s => s.OpeningTime.HasValue ? s.OpeningTime.Value.ToString(@"hh\:mm") : null))
            .ForMember(d => d.ClosingTime, o => o.MapFrom(s => s.ClosingTime.HasValue ? s.ClosingTime.Value.ToString(@"hh\:mm") : null))
            .ForMember(d => d.Services, o => o.MapFrom(s => s.Services.Where(x => x.IsActive)))
            .ForMember(d => d.StaffMembers, o => o.MapFrom(s => s.StaffMembers.Where(x => x.IsActive)));

        // Service -> PublicServiceDto (+ staff mini list)
        CreateMap<Service, PublicServiceDto>()
            .ForMember(d => d.Staff, o => o.MapFrom(s => s.Staffs.Where(x => x.IsActive)));

        // Staff -> PublicStaffDto (+ service mini list)
        CreateMap<Staff, PublicStaffDto>()
            .ForMember(d => d.Services, o => o.MapFrom(s => s.Services.Where(x => x.IsActive)));

        // mini mappers
        CreateMap<Staff, PublicStaffMiniDto>();
        CreateMap<Service, PublicServiceMiniDto>();
    }
}
