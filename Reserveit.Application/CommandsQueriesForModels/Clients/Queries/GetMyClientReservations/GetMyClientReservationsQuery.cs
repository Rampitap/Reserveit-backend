using MediatR;
using Reserveit.Application.Common.DTOs.ReservationsDtos;
using Reserveit.Application.Common.Pagination;
using Reserveit.Domain.Enums;

namespace Reserveit.Application.CommandsQueriesForModels.Clients.Queries.GetMyClientReservations;

public sealed class GetMyClientReservationsQuery : IRequest<PagedResult<ReservationDto>>
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 12;

    
    public ReservationStatus? Status { get; init; }
}
