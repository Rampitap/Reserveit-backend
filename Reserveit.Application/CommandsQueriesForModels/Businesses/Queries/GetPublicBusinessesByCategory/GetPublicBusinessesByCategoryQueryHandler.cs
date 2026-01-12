using AutoMapper;
using MediatR;
using Reserveit.Application.Common.DTOs.BuisnessDtos;
using Reserveit.Application.Common.Pagination;
using Reserveit.Domain.Interfaces;

namespace Reserveit.Application.CommandsQueriesForModels.Businesses.Queries.GetPublicBusinessesByCategory;

public sealed class GetPublicBusinessesByCategoryQueryHandler
    : IRequestHandler<GetPublicBusinessesByCategoryQuery, PagedResult<PublicBusinessCardDto>>
{
    private readonly IBusinessRepository _businessRepository;
    private readonly IMapper _mapper;

    public GetPublicBusinessesByCategoryQueryHandler(IBusinessRepository businessRepository, IMapper mapper)
    {
        _businessRepository = businessRepository;
        _mapper = mapper;
    }

    public async Task<PagedResult<PublicBusinessCardDto>> Handle(GetPublicBusinessesByCategoryQuery request, CancellationToken ct)
    {
        var page = request.Page < 1 ? 1 : request.Page;
        var pageSize = request.PageSize < 1 ? 12 : request.PageSize;

        var items = await _businessRepository.GetPublicByCategoryAsync(
            page, pageSize, request.Q, request.CategoryId, request.Category, ct);

        var total = await _businessRepository.CountPublicByCategoryAsync(
            request.Q, request.CategoryId, request.Category, ct);

        return new PagedResult<PublicBusinessCardDto>
        {
            Page = page,
            PageSize = pageSize,
            Total = total,
            Items = items.Select(_mapper.Map<PublicBusinessCardDto>).ToList()
        };
    }
}
