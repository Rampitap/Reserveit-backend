using AutoMapper;
using MediatR;
using Reserveit.Application.Common.DTOs.BuisnessDtos;
using Reserveit.Application.Common.Pagination;
using Reserveit.Domain.Interfaces;

namespace Reserveit.Application.CommandsQueriesForModels.Businesses.Queries.GetAllPublicBusinesses;

public sealed class GetPublicBusinessesQueryHandler
    : IRequestHandler<GetPublicBusinessesQuery, PagedResult<PublicBusinessSummaryDto>>
{
    private readonly IBusinessRepository _repo;
    private readonly IMapper _mapper;

    public GetPublicBusinessesQueryHandler(IBusinessRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<PagedResult<PublicBusinessSummaryDto>> Handle(GetPublicBusinessesQuery request, CancellationToken ct)
    {
        var items = await _repo.SearchPublicAsync(request.Page, request.PageSize, request.Q, ct);
        var total = await _repo.CountPublicAsync(request.Q, ct);

        return new PagedResult<PublicBusinessSummaryDto>
        {
            Page = request.Page < 1 ? 1 : request.Page,
            PageSize = request.PageSize < 1 ? 12 : request.PageSize,
            Total = total,
            Items = items.Select(_mapper.Map<PublicBusinessSummaryDto>).ToList()
        };
    }
}
