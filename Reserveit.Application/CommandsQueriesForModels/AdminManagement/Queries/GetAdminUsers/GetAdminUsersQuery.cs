using MediatR;
using Reserveit.Application.Common.DTOs.AdminManageDtos;
using Reserveit.Application.Common.Pagination;

namespace Reserveit.Application.CommandsQueriesForModels.AdminManagement.Queries.GetAdminUsers;

public sealed record GetAdminUsersQuery(
    int Page = 1,
    int PageSize = 20,
    string? Q = null
) : IRequest<PagedResult<AdminUserDto>>;