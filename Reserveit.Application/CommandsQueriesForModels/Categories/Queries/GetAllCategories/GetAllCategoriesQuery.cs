using MediatR;
using Reserveit.Application.Common.DTOs.CategoryDtos;

namespace Reserveit.Application.CommandsQueriesForModels.Categories.Queries.GetAllCategories;

public sealed record GetAllCategoriesQuery() : IRequest<IReadOnlyList<CategoryDto>>;
