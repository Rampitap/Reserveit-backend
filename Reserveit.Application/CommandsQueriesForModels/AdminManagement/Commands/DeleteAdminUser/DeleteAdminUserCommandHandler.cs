using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Reserveit.Application.Interfaces;

namespace Reserveit.Application.CommandsQueriesForModels.AdminManagement.Commands.DeleteAdminUser;

public sealed class DeleteAdminUserCommandHandler : IRequestHandler<DeleteAdminUserCommand>
{
    private readonly IAdminUserRepository _repo;
    private readonly IValidator<DeleteAdminUserCommand> _validator;
    private readonly ILogger<DeleteAdminUserCommandHandler> _logger;

    public DeleteAdminUserCommandHandler(
        IAdminUserRepository repo,
        IValidator<DeleteAdminUserCommand> validator,
        ILogger<DeleteAdminUserCommandHandler> logger)
    {
        _repo = repo;
        _validator = validator;
        _logger = logger;
    }

    public async Task Handle(DeleteAdminUserCommand request, CancellationToken ct)
    {
        var vr = await _validator.ValidateAsync(request, ct);
        if (!vr.IsValid) throw new ValidationException(vr.Errors);

        await _repo.DeleteAsync(request.UserId, ct);

        _logger.LogInformation("Admin deleted user. UserId={UserId}", request.UserId);
    }
}