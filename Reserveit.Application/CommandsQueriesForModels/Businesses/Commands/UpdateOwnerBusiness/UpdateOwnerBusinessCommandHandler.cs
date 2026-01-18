using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Reserveit.Application.CurrentUserService;
using Reserveit.Domain.Entities;
using Reserveit.Domain.Exceptions;
using Reserveit.Domain.Interfaces;

namespace Reserveit.Application.CommandsQueriesForModels.Businesses.Commands.UpdateOwnerBusiness;

public sealed class UpdateOwnerBusinessCommandHandler : IRequestHandler<UpdateOwnerBusinessCommand>
{
    private readonly ICurrentUser _currentUser;
    private readonly IBusinessRepository _repo;
    private readonly IValidator<UpdateOwnerBusinessCommand> _validator;
    private readonly ILogger<UpdateOwnerBusinessCommandHandler> _logger;

    public UpdateOwnerBusinessCommandHandler(
        ICurrentUser currentUser,
        IBusinessRepository repo,
        IValidator<UpdateOwnerBusinessCommand> validator,
        ILogger<UpdateOwnerBusinessCommandHandler> logger)
    {
        _currentUser = currentUser;
        _repo = repo;
        _validator = validator;
        _logger = logger;
    }

    public async Task Handle(UpdateOwnerBusinessCommand request, CancellationToken ct)
    {
        var vr = await _validator.ValidateAsync(request, ct);
        if (!vr.IsValid) throw new ValidationException(vr.Errors);

        var business = await _repo.GetByIdAsync(request.BusinessId, ct)
            ?? throw new NotFoundException(nameof(Business), request.BusinessId.ToString());

        if (business.OwnerId != _currentUser.UserId)
            throw new ForbiddenException("You don't have access to this business.");

        business.Name = request.Data.Name;
        business.Address = request.Data.Address;
        business.Timezone = request.Data.Timezone;
        business.OpeningTime = request.Data.OpeningTime;
        business.ClosingTime = request.Data.ClosingTime;
        business.ImageUrl = request.Data.ImageUrl;
        business.CancellationPolicyJson = request.Data.CancellationPolicyJson;
        business.UpdatedAt = DateTimeOffset.UtcNow;

        await _repo.SaveChangesAsync(ct);

        _logger.LogInformation("Owner updated business. BusinessId={BusinessId}, OwnerId={OwnerId}",
            business.Id, business.OwnerId);
    }
}
