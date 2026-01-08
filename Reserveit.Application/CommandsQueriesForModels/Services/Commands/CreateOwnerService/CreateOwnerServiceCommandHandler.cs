using FluentValidation;
using MediatR;
using Reserveit.Application.CurrentUserService;
using Reserveit.Domain.Entities;
using Reserveit.Domain.Exceptions;
using Reserveit.Domain.Interfaces;

namespace Reserveit.Application.CommandsQueriesForModels.Services.Commands.CreateOwnerService;

public sealed class CreateOwnerServiceCommandHandler : IRequestHandler<CreateOwnerServiceCommand, Guid>
{
    private readonly ICurrentUser _currentUser;
    private readonly IBusinessRepository _businessRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IValidator<CreateOwnerServiceCommand> _validator;

    public CreateOwnerServiceCommandHandler(
        ICurrentUser currentUser,
        IBusinessRepository businessRepository,
        IServiceRepository serviceRepository,
        IValidator<CreateOwnerServiceCommand> validator)
    {
        _currentUser = currentUser;
        _businessRepository = businessRepository;
        _serviceRepository = serviceRepository;
        _validator = validator;
    }

    public async Task<Guid> Handle(CreateOwnerServiceCommand request, CancellationToken ct)
    {
        var vr = await _validator.ValidateAsync(request, ct);
        if (!vr.IsValid) throw new ValidationException(vr.Errors);

        var ownerId = _currentUser.UserId;
        if (!await _businessRepository.IsOwnedByAsync(request.BusinessId, ownerId, ct))
            throw new ForbiddenException("You can't manage this business.");

        var service = new Service
        {
            Id = Guid.NewGuid(),
            BusinessId = request.BusinessId,
            Name = request.Data.Name.Trim(),
            Description = request.Data.Description?.Trim(),
            DurationMinutes = request.Data.DurationMinutes,
            Price = request.Data.Price,
            IsActive = true,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        await _serviceRepository.AddAsync(service, ct);
        await _serviceRepository.SaveChangesAsync(ct);

        return service.Id;
    }
}
