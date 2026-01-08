using FluentValidation;
using MediatR;
using Reserveit.Application.CurrentUserService;
using Reserveit.Domain.Exceptions;
using Reserveit.Domain.Interfaces;

namespace Reserveit.Application.CommandsQueriesForModels.Services.Commands.DeactivateOwnerService;

public sealed class DeactivateOwnerServiceCommandHandler : IRequestHandler<DeactivateOwnerServiceCommand>
{
    private readonly ICurrentUser _currentUser;
    private readonly IBusinessRepository _businessRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IValidator<DeactivateOwnerServiceCommand> _validator;

    public DeactivateOwnerServiceCommandHandler(
        ICurrentUser currentUser,
        IBusinessRepository businessRepository,
        IServiceRepository serviceRepository,
        IValidator<DeactivateOwnerServiceCommand> validator)
    {
        _currentUser = currentUser;
        _businessRepository = businessRepository;
        _serviceRepository = serviceRepository;
        _validator = validator;
    }

    public async Task Handle(DeactivateOwnerServiceCommand request, CancellationToken ct)
    {
        var vr = await _validator.ValidateAsync(request, ct);
        if (!vr.IsValid) throw new ValidationException(vr.Errors);

        var ownerId = _currentUser.UserId;
        if (!await _businessRepository.IsOwnedByAsync(request.BusinessId, ownerId, ct))
            throw new ForbiddenException("You can't manage this business.");

        var service = await _serviceRepository.GetTrackedByIdAsync(request.ServiceId, ct)
            ?? throw new NotFoundException("Service", request.ServiceId.ToString());

        if (service.BusinessId != request.BusinessId)
            throw new ForbiddenException("Service doesn't belong to this business.");

        service.IsActive = false;
        service.UpdatedAt = DateTimeOffset.UtcNow;

        await _serviceRepository.SaveChangesAsync(ct);
    }
}
