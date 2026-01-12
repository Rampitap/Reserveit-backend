using MediatR;
using Reserveit.Application.Common.DTOs.BuisnessDtos;
using Reserveit.Application.Common.Pagination;

namespace Reserveit.Application.CommandsQueriesForModels.Businesses.Queries.GetPublicBusinessesByCategory;

public sealed record GetPublicBusinessesByCategoryQuery(
    int Page = 1,
    int PageSize = 12,
    string? Q = null,
    Guid? CategoryId = null,
    string? Category = null
) : IRequest<PagedResult<PublicBusinessCardDto>>;
