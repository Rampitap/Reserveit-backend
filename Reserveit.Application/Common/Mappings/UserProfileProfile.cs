using AutoMapper;
using Reserveit.Application.Common.DTOs.UserDtos;
using Reserveit.Domain.Entities;

namespace Reserveit.Application.Common.Mappings;

public class UserProfileProfile : Profile
{
    public UserProfileProfile()
    {
        CreateMap<User, UserProfileDto>()
            .ForMember(d => d.Roles, o => o.Ignore())
            .ForMember(d => d.Email, o => o.MapFrom(s => s.Email!));
    }
}
