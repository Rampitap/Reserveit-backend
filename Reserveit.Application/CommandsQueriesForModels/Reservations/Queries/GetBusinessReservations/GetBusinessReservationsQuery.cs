using MediatR;
using Reserveit.Application.Common.DTOs.ReservationsDtos;
using Reserveit.Application.Common.Pagination;
using Reserveit.Domain.Enums;

namespace Reserveit.Application.CommandsQueriesForModels.Reservations.Queries.GetBusinessReservations;

public sealed record GetBusinessReservationsQuery(
    Guid BusinessId,
    DateTimeOffset From,
    DateTimeOffset To,
    ReservationStatus? Status,
    Guid? StaffId,
    int Page = 1,
    int PageSize = 20
) : IRequest<PagedResult<OwnerReservationDto>>;
