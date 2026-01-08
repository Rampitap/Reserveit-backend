using FluentValidation;
using MediatR;
using Reserveit.Application.CurrentUserService;
using Reserveit.Application.Interfaces;
using Reserveit.Domain.Exceptions;
using Reserveit.Domain.Interfaces;

namespace Reserveit.Application.CommandsQueriesForModels.Staffes.Commands.UpdateOwnerStaffStatus;

public sealed class UpdateOwnerStaffStatusCommandHandler : IRequestHandler<UpdateOwnerStaffStatusCommand>
{
    private readonly ICurrentUser _currentUser;
    private readonly IBusinessRepository _businessRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IUserAccountService _userAccount;
    private readonly IValidator<UpdateOwnerStaffStatusCommand> _validator;

    public UpdateOwnerStaffStatusCommandHandler(
        ICurrentUser currentUser,
        IBusinessRepository businessRepo,
        IStaffRepository staffRepo,
        IUserAccountService userAccount,
        IValidator<UpdateOwnerStaffStatusCommand> validator)
    {
        _currentUser = currentUser;
        _businessRepo = businessRepo;
        _staffRepo = staffRepo;
        _userAccount = userAccount;
        _validator = validator;
    }

    public async Task Handle(UpdateOwnerStaffStatusCommand request, CancellationToken ct)
    {
        var vr = await _validator.ValidateAsync(request, ct);
        if (!vr.IsValid) throw new ValidationException(vr.Errors);

        if (!await _businessRepo.IsOwnedByAsync(request.BusinessId, _currentUser.UserId, ct))
            throw new ForbiddenException("You can't manage this business.");

        var staff = await _staffRepo.GetTrackedByBusinessAndIdAsync(request.BusinessId, request.StaffId, ct)
            ?? throw new NotFoundException("Staff", request.StaffId.ToString());

        staff.IsActive = request.Data.IsActive;
        staff.UpdatedAt = DateTimeOffset.UtcNow;
        await _staffRepo.SaveChangesAsync(ct);

        //  User.IsActive
        if (staff.UserId.HasValue)
            await _userAccount.SetIsActiveAsync(staff.UserId.Value, request.Data.IsActive, ct);
    }
}
