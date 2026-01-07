using MediatR;
using Reserveit.Application.Common.DTOs.BuisnessDtos;
using Reserveit.Application.Common.Pagination;

namespace Reserveit.Application.CommandsQueriesForModels.Businesses.Queries.GetAllPublicBusinesses;

public sealed record GetPublicBusinessesQuery(int Page = 1, int PageSize = 12, string? Q = null)
    : IRequest<PagedResult<PublicBusinessSummaryDto>>;
