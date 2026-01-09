using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Reserveit.Application.Common.DTOs.AdminManageDtos;
using Reserveit.Application.Common.Pagination;
using Reserveit.Application.Interfaces;

namespace Reserveit.Application.CommandsQueriesForModels.AdminManagement.Queries.GetAdminUsers;

public sealed class GetAdminUsersQueryHandler
    : IRequestHandler<GetAdminUsersQuery, PagedResult<AdminUserDto>>
{
    private readonly IAdminUserRepository _repo;
    private readonly IValidator<GetAdminUsersQuery> _validator;
    private readonly ILogger<GetAdminUsersQueryHandler> _logger;

    public GetAdminUsersQueryHandler(
        IAdminUserRepository repo,
        IValidator<GetAdminUsersQuery> validator,
        ILogger<GetAdminUsersQueryHandler> logger)
    {
        _repo = repo;
        _validator = validator;
        _logger = logger;
    }

    public async Task<PagedResult<AdminUserDto>> Handle(GetAdminUsersQuery request, CancellationToken ct)
    {
        var vr = await _validator.ValidateAsync(request, ct);
        if (!vr.IsValid) throw new ValidationException(vr.Errors);

        var result = await _repo.GetPagedAsync(request.Page, request.PageSize, request.Q, ct);

        _logger.LogInformation("Admin fetched users. Page={Page}, PageSize={PageSize}, Total={Total}",
            result.Page, result.PageSize, result.Total);

        return result;
    }
}
