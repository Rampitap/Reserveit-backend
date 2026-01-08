using FluentValidation;
using MediatR;
using Reserveit.Application.CurrentUserService;
using Reserveit.Domain.Exceptions;
using Reserveit.Domain.Interfaces;
using System;

namespace Reserveit.Application.CommandsQueriesForModels.Services.Commands.AddStaffToService;

public sealed class AddStaffToServiceCommandHandler : IRequestHandler<AddStaffToServiceCommand>
{
    private readonly ICurrentUser _currentUser;
    private readonly IBusinessRepository _businessRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IValidator<AddStaffToServiceCommand> _validator;

    public AddStaffToServiceCommandHandler(
        ICurrentUser currentUser,
        IBusinessRepository businessRepository,
        IServiceRepository serviceRepository,
        IValidator<AddStaffToServiceCommand> validator)
    {
        _currentUser = currentUser;
        _businessRepository = businessRepository;
        _serviceRepository = serviceRepository;
        _validator = validator;
    }

    public async Task Handle(AddStaffToServiceCommand request, CancellationToken ct)
    {
       
    
        var vr = await _validator.ValidateAsync(request, ct);
        if (!vr.IsValid) throw new ValidationException(vr.Errors);

        var ownerId = _currentUser.UserId;

        if (!await _businessRepository.IsOwnedByAsync(request.BusinessId, ownerId, ct))
            throw new ForbiddenException("You can't manage this business.");

        await _serviceRepository.AddStaffToServiceAsync(request.BusinessId, request.ServiceId, request.StaffId, ct);
    }
}
