using AutoMapper;
using Reserveit.Application.Common.DTOs.CategoryDtos;
using Reserveit.Domain.Entities;

namespace Reserveit.Application.Common.Mappings;

public sealed class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<Category, CategoryDto>();
    }
}
