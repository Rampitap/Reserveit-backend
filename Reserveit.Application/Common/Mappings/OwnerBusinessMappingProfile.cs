using AutoMapper;
using Reserveit.Application.Common.DTOs.BuisnessDtos;
using Reserveit.Domain.Entities;

namespace Reserveit.Application.Common.Mappings;

public sealed class OwnerBusinessMappingProfile : Profile
{
    public OwnerBusinessMappingProfile()
    {
        CreateMap<Business, OwnerBusinessSummaryDto>()
            .ForMember(d => d.OpeningTime,
                o => o.MapFrom(s => s.OpeningTime.HasValue ? s.OpeningTime.Value.ToString(@"hh\:mm") : null))
            .ForMember(d => d.ClosingTime,
                o => o.MapFrom(s => s.ClosingTime.HasValue ? s.ClosingTime.Value.ToString(@"hh\:mm") : null))
            .ForMember(d => d.ImageUrl,
                o => o.MapFrom(s => s.ImageUrl))
             .ForMember(d => d.CategoryName,
                o => o.MapFrom(s => s.Category != null ? s.Category.Name : null));
    }
}
