using AutoMapper;
using Reserveit.Application.Common.DTOs.AuthDtod;
using Reserveit.Domain.Entities;

namespace Reserveit.Application.Common.Mappings;

public class AuthProfile : Profile
{
    public AuthProfile()
    {
        //for new user creation
        CreateMap<RegisterDto, User>()
            
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))

            
            .ForSourceMember(src => src.Password, opt => opt.DoNotValidate())
            .ForSourceMember(src => src.ConfirmPassword, opt => opt.DoNotValidate())

            // role is ignores
            .ForSourceMember(src => src.Role, opt => opt.DoNotValidate());


        // answers to frontend after registration or login
        CreateMap<User, ResponseDto>()
            // Id, Email, FirstName, LastName maps automatically

            // role is ignored.
            .ForMember(dest => dest.Roles, opt => opt.Ignore());
    }
}
