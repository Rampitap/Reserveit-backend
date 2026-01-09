using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Reserveit.Application.CurrentUserService;
using Reserveit.Domain.Entities;
using Reserveit.Domain.Exceptions;
using Reserveit.Domain.Interfaces;

namespace Reserveit.Application.CommandsQueriesForModels.Businesses.Commands.DeleteOwnerBusiness;

public sealed class DeleteOwnerBusinessCommandHandler : IRequestHandler<DeleteOwnerBusinessCommand>
{
    private readonly ICurrentUser _currentUser;
    private readonly IBusinessRepository _repo;
    private readonly IValidator<DeleteOwnerBusinessCommand> _validator;
    private readonly ILogger<DeleteOwnerBusinessCommandHandler> _logger;

    public DeleteOwnerBusinessCommandHandler(
        ICurrentUser currentUser,
        IBusinessRepository repo,
        IValidator<DeleteOwnerBusinessCommand> validator,
        ILogger<DeleteOwnerBusinessCommandHandler> logger)
    {
        _currentUser = currentUser;
        _repo = repo;
        _validator = validator;
        _logger = logger;
    }

    public async Task Handle(DeleteOwnerBusinessCommand request, CancellationToken ct)
    {
        var vr = await _validator.ValidateAsync(request, ct);
        if (!vr.IsValid) throw new ValidationException(vr.Errors);

        var business = await _repo.GetByIdAsync(request.BusinessId, ct)
            ?? throw new NotFoundException(nameof(Business), request.BusinessId.ToString());

        if (business.OwnerId != _currentUser.UserId)
            throw new ForbiddenException("You don't have access to this business.");

        var hasFutureConfirmed = await _repo.HasFutureConfirmedReservationsAsync(
            business.Id, DateTimeOffset.UtcNow, ct);

        if (hasFutureConfirmed)
            throw new InvalidOperationException("You can't delete business because it has future confirmed reservations.");

        await _repo.DeleteAsync(business, ct);
        await _repo.SaveChangesAsync(ct);

        _logger.LogInformation("Owner deleted business. BusinessId={BusinessId}, OwnerId={OwnerId}",
            business.Id, business.OwnerId);
    }
}
