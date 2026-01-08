using FluentValidation;
using MediatR;
using Reserveit.Application.CurrentUserService;
using Reserveit.Domain.Exceptions;
using Reserveit.Domain.Interfaces;

namespace Reserveit.Application.CommandsQueriesForModels.Services.Commands.UpdateOwnerService;

public sealed class UpdateOwnerServiceCommandHandler : IRequestHandler<UpdateOwnerServiceCommand>
{
    private readonly ICurrentUser _currentUser;
    private readonly IBusinessRepository _businessRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IValidator<UpdateOwnerServiceCommand> _validator;

    public UpdateOwnerServiceCommandHandler(
        ICurrentUser currentUser,
        IBusinessRepository businessRepository,
        IServiceRepository serviceRepository,
        IValidator<UpdateOwnerServiceCommand> validator)
    {
        _currentUser = currentUser;
        _businessRepository = businessRepository;
        _serviceRepository = serviceRepository;
        _validator = validator;
    }

    public async Task Handle(UpdateOwnerServiceCommand request, CancellationToken ct)
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

        service.Name = request.Data.Name.Trim();
        service.Description = request.Data.Description?.Trim();
        service.DurationMinutes = request.Data.DurationMinutes;
        service.Price = request.Data.Price;
        service.IsActive = request.Data.IsActive;
        service.UpdatedAt = DateTimeOffset.UtcNow;

        await _serviceRepository.SaveChangesAsync(ct);
    }
}