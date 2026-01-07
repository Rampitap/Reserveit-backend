using FluentValidation;
using MediatR;
using Reserveit.Application.CommandsQueriesForModels.Staffes.Commands.ChangeReservationStatus;
using Reserveit.Application.CurrentUserService;
using Reserveit.Application.Interfaces;
using Reserveit.Domain.Exceptions;
using Reserveit.Domain.Interfaces;

namespace Reserveit.Application.CommandsQueriesForModels.Staffes.Commands.CreateStaffByOwner;

public sealed class CreateStaffCommandHandler : IRequestHandler<CreateStaffCommand, Guid>
{
    private readonly ICurrentUser _currentUser;
    private readonly IBusinessRepository _businessRepository;
    private readonly IStaffAccountService _staffAccountService;
    private readonly IValidator<CreateStaffCommand> _validator;

    public CreateStaffCommandHandler(
        ICurrentUser currentUser,
        IBusinessRepository businessRepository,
        IStaffAccountService staffAccountService,
        IValidator<CreateStaffCommand> validator)
    {
        _currentUser = currentUser;
        _businessRepository = businessRepository;
        _staffAccountService = staffAccountService;
        _validator = validator;
    }

    public async Task<Guid> Handle(CreateStaffCommand request, CancellationToken ct)
    {
        var vr = await _validator.ValidateAsync(request, ct);
        if (!vr.IsValid)
            throw new ValidationException(vr.Errors);

        // owner id from context
        var ownerId = _currentUser.UserId;

        // ✅ here is owner-check
        var allowed = await _businessRepository.IsOwnedByAsync(request.Data.BusinessId, ownerId, ct);
        if (!allowed)
            throw new ForbiddenException("You can't manage this business.");

        return await _staffAccountService.CreateStaffAsync(request.Data, ct);
    }
}
