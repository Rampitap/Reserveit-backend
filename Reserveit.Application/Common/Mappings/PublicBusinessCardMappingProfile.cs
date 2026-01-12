using AutoMapper;
using Reserveit.Application.Common.DTOs.BuisnessDtos;
using Reserveit.Domain.Entities;

namespace Reserveit.Application.Common.Mappings;

public sealed class PublicBusinessCardMappingProfile : Profile
{
    public PublicBusinessCardMappingProfile()
    {
        CreateMap<Business, PublicBusinessCardDto>()
            .ForMember(d => d.CategoryName,
                o => o.MapFrom(s => s.Category != null ? s.Category.Name : null));
    }
}
