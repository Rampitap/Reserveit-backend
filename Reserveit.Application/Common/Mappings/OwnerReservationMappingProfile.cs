using AutoMapper;
using Reserveit.Application.Common.DTOs.ReservationsDtos;
using Reserveit.Domain.Entities;

namespace Reserveit.Application.Common.Mappings;

public sealed class OwnerReservationMappingProfile : Profile
{
    public OwnerReservationMappingProfile()
    {
        CreateMap<Reservation, OwnerReservationDto>()
            .ForMember(d => d.BusinessName, o => o.MapFrom(s => s.Business.Name))
            .ForMember(d => d.BusinessAddress, o => o.MapFrom(s => s.Business.Address))
            .ForMember(d => d.ServiceName, o => o.MapFrom(s => s.Service.Name))
            .ForMember(d => d.Price, o => o.MapFrom(s => s.Service.Price))
            .ForMember(d => d.DurationMinutes, o => o.MapFrom(s => s.Service.DurationMinutes))
            .ForMember(d => d.StaffName, o => o.MapFrom(s => s.Staff != null ? s.Staff.DisplayName : null))
            .ForMember(d => d.ClientName, o => o.MapFrom(s => (s.Client.FirstName + " " + s.Client.LastName).Trim()))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));
    }
}
