using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Reserveit.Application.CurrentUserService;
using Reserveit.Domain.Entities;
using Reserveit.Domain.Exceptions;
using Reserveit.Domain.Interfaces;

namespace Reserveit.Application.CommandsQueriesForModels.Businesses.Commands.UpdateOwnerBusinessStatus;

public sealed class UpdateOwnerBusinessStatusCommandHandler : IRequestHandler<UpdateOwnerBusinessStatusCommand>
{
    private readonly ICurrentUser _currentUser;
    private readonly IBusinessRepository _repo;
    private readonly IValidator<UpdateOwnerBusinessStatusCommand> _validator;
    private readonly ILogger<UpdateOwnerBusinessStatusCommandHandler> _logger;

    public UpdateOwnerBusinessStatusCommandHandler(
        ICurrentUser currentUser,
        IBusinessRepository repo,
        IValidator<UpdateOwnerBusinessStatusCommand> validator,
        ILogger<UpdateOwnerBusinessStatusCommandHandler> logger)
    {
        _currentUser = currentUser;
        _repo = repo;
        _validator = validator;
        _logger = logger;
    }

    public async Task Handle(UpdateOwnerBusinessStatusCommand request, CancellationToken ct)
    {
        var vr = await _validator.ValidateAsync(request, ct);
        if (!vr.IsValid) throw new ValidationException(vr.Errors);

        var business = await _repo.GetByIdAsync(request.BusinessId, ct)
            ?? throw new NotFoundException(nameof(Business), request.BusinessId.ToString());

        if (business.OwnerId != _currentUser.UserId)
            throw new ForbiddenException("You don't have access to this business.");

        business.IsActive = request.IsActive;
        business.UpdatedAt = DateTimeOffset.UtcNow;

        await _repo.SaveChangesAsync(ct);

        _logger.LogInformation("Owner changed business status. BusinessId={BusinessId}, IsActive={IsActive}",
            business.Id, business.IsActive);
    }
}
