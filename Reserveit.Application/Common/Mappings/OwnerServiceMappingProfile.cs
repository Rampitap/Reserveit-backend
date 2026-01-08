using AutoMapper;
using Reserveit.Application.Common.DTOs.ServiceDtos;
using Reserveit.Domain.Entities;

namespace Reserveit.Application.Common.Mappings;

public sealed class OwnerServiceMappingProfile : Profile
{
    public OwnerServiceMappingProfile()
    {
        CreateMap<Service, OwnerServiceDto>();
    }
}
