using MediatR;
using Reserveit.Application.Common.DTOs.ReservationsDtos;
using Reserveit.Application.Common.Pagination;
using Reserveit.Domain.Enums;

namespace Reserveit.Application.CommandsQueriesForModels.Staffes.Queries.GetMyStaffReservations;

public sealed record GetMyStaffReservationsQuery : IRequest<PagedResult<ReservationDto>>
{
    public DateTimeOffset From { get; init; }
    public DateTimeOffset To { get; init; }

    public ReservationStatus? Status { get; init; }
    public ReservationSort Sort { get; init; } = ReservationSort.StartAsc;

    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 12;
}
