using AutoMapper;
using MediatR;
using Reserveit.Application.Common.DTOs.CategoryDtos;
using Reserveit.Domain.Interfaces;

namespace Reserveit.Application.CommandsQueriesForModels.Categories.Queries.GetAllCategories;

public sealed class GetAllCategoriesQueryHandler
    : IRequestHandler<GetAllCategoriesQuery, IReadOnlyList<CategoryDto>>
{
    private readonly ICategoryRepository _repo;
    private readonly IMapper _mapper;

    public GetAllCategoriesQueryHandler(ICategoryRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken ct)
        => (await _repo.GetAllAsync(ct)).Select(_mapper.Map<CategoryDto>).ToList();
}
