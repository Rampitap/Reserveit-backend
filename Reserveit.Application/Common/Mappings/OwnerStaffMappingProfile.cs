using AutoMapper;
using Reserveit.Application.Common.DTOs.StaffDtos;
using Reserveit.Domain.Entities;

namespace Reserveit.Application.Common.Mappings;

public sealed class OwnerStaffMappingProfile : Profile
{
    public OwnerStaffMappingProfile()
    {
        CreateMap<Staff, OwnerStaffDto>();
    }
}
