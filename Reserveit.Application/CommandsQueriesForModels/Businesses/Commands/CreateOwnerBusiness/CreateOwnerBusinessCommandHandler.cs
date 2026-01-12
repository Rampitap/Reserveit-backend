using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Reserveit.Application.CurrentUserService;
using Reserveit.Domain.Entities;
using Reserveit.Domain.Interfaces;

namespace Reserveit.Application.CommandsQueriesForModels.Businesses.Commands.CreateOwnerBusiness;

public sealed class CreateOwnerBusinessCommandHandler : IRequestHandler<CreateOwnerBusinessCommand, Guid>
{
    private readonly ICurrentUser _currentUser;
    private readonly IBusinessRepository _repo;
    private readonly IValidator<CreateOwnerBusinessCommand> _validator;
    private readonly ILogger<CreateOwnerBusinessCommandHandler> _logger;

    public CreateOwnerBusinessCommandHandler(
        ICurrentUser currentUser,
        IBusinessRepository repo,
        IValidator<CreateOwnerBusinessCommand> validator,
        ILogger<CreateOwnerBusinessCommandHandler> logger)
    {
        _currentUser = currentUser;
        _repo = repo;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateOwnerBusinessCommand request, CancellationToken ct)
    {
        var vr = await _validator.ValidateAsync(request, ct);
        if (!vr.IsValid) throw new ValidationException(vr.Errors);

        var b = new Business
        {
            Id = Guid.NewGuid(),
            OwnerId = _currentUser.UserId,
            Name = request.Data.Name,
            Address = request.Data.Address,
            Timezone = request.Data.Timezone,
            OpeningTime = request.Data.OpeningTime,
            ClosingTime = request.Data.ClosingTime,
            CancellationPolicyJson = request.Data.CancellationPolicyJson,
            CategoryId = request.Data.CategoryId,
            IsActive = true,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        await _repo.AddAsync(b, ct);
        await _repo.SaveChangesAsync(ct);

        _logger.LogInformation("Owner created business. BusinessId={BusinessId}, OwnerId={OwnerId}", b.Id, b.OwnerId);

        return b.Id;
    }
}
