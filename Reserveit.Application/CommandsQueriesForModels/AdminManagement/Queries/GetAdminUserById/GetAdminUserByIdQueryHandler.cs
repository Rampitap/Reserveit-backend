using FluentValidation;
using MediatR;
using Reserveit.Application.Common.DTOs.AdminManageDtos;
using Reserveit.Application.Interfaces;

namespace Reserveit.Application.CommandsQueriesForModels.AdminManagement.Queries.GetAdminUserById;

public sealed class GetAdminUserByIdQueryHandler : IRequestHandler<GetAdminUserByIdQuery, AdminUserDto>
{
    private readonly IAdminUserRepository _repo;
    private readonly IValidator<GetAdminUserByIdQuery> _validator;

    public GetAdminUserByIdQueryHandler(IAdminUserRepository repo, IValidator<GetAdminUserByIdQuery> validator)
    {
        _repo = repo;
        _validator = validator;
    }

    public async Task<AdminUserDto> Handle(GetAdminUserByIdQuery request, CancellationToken ct)
    {
        var vr = await _validator.ValidateAsync(request, ct);
        if (!vr.IsValid) throw new ValidationException(vr.Errors);

        return await _repo.GetByIdAsync(request.UserId, ct);
    }
}
