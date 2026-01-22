using AutoMapper;
using Reserveit.Application.Common.DTOs.ReservationsDtos;
using Reserveit.Domain.Entities;

namespace Reserveit.Application.Common.Mappings;

public sealed class ReservationMappingProfile : Profile
{
    public ReservationMappingProfile()
    {
        CreateMap<Reservation, ReservationDto>()
            .ForMember(d => d.BusinessName, o => o.MapFrom(s => s.Business.Name))
            .ForMember(d => d.BusinessAddress, o => o.MapFrom(s => s.Business.Address))
            .ForMember(d => d.ServiceName, o => o.MapFrom(s => s.Service.Name))
            .ForMember(d => d.Price, o => o.MapFrom(s => s.Service.Price))
            .ForMember(d => d.DurationMinutes, o => o.MapFrom(s => s.Service.DurationMinutes))
            .ForMember(d => d.StaffName, o => o.MapFrom(s => s.Staff != null ? s.Staff.DisplayName : null))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.ClientId, o => o.MapFrom(s => s.ClientId))
            .ForMember(d => d.ClientName, o => o.MapFrom(s =>
                s.Client != null
                    ? ((s.Client.FirstName + " " + s.Client.LastName).Trim())
                    : string.Empty
            ))
            .ForMember(d => d.ClientEmail, o => o.MapFrom(s => s.Client != null ? s.Client.Email : null))
            .ForMember(d => d.StaffEmail, o => o.MapFrom(s => s.Staff != null && s.Staff.User != null ? s.Staff.User.Email : null)); 
    }
}

