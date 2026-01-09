using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Reserveit.Application.Interfaces;

namespace Reserveit.Application.CommandsQueriesForModels.AdminManagement.Commands.UpdateAdminUserStatus;

public sealed class UpdateAdminUserStatusCommandHandler : IRequestHandler<UpdateAdminUserStatusCommand>
{
    private readonly IAdminUserRepository _repo;
    private readonly IValidator<UpdateAdminUserStatusCommand> _validator;
    private readonly ILogger<UpdateAdminUserStatusCommandHandler> _logger;

    public UpdateAdminUserStatusCommandHandler(
        IAdminUserRepository repo,
        IValidator<UpdateAdminUserStatusCommand> validator,
        ILogger<UpdateAdminUserStatusCommandHandler> logger)
    {
        _repo = repo;
        _validator = validator;
        _logger = logger;
    }

    public async Task Handle(UpdateAdminUserStatusCommand request, CancellationToken ct)
    {
        var vr = await _validator.ValidateAsync(request, ct);
        if (!vr.IsValid) throw new ValidationException(vr.Errors);

        await _repo.UpdateIsActiveAsync(request.UserId, request.IsActive, ct);

        _logger.LogInformation("Admin changed user status. UserId={UserId}, IsActive={IsActive}",
            request.UserId, request.IsActive);
    }
}
