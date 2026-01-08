using FluentValidation;
using MediatR;
using Reserveit.Application.CurrentUserService;
using Reserveit.Application.Interfaces;
using Reserveit.Domain.Exceptions;
using Reserveit.Domain.Interfaces;

namespace Reserveit.Application.CommandsQueriesForModels.Staffes.Commands.DeleteOwnerStaff;

public sealed class DeleteOwnerStaffCommandHandler : IRequestHandler<DeleteOwnerStaffCommand>
{
    private readonly ICurrentUser _currentUser;
    private readonly IBusinessRepository _businessRepo;
    private readonly IStaffRepository _staffRepo;
    private readonly IReservationRepository _reservationRepo;
    private readonly IUserAccountService _userAccount;
    private readonly IValidator<DeleteOwnerStaffCommand> _validator;

    public DeleteOwnerStaffCommandHandler(
        ICurrentUser currentUser,
        IBusinessRepository businessRepo,
        IStaffRepository staffRepo,
        IReservationRepository reservationRepo,
        IUserAccountService userAccount,
        IValidator<DeleteOwnerStaffCommand> validator)
    {
        _currentUser = currentUser;
        _businessRepo = businessRepo;
        _staffRepo = staffRepo;
        _reservationRepo = reservationRepo;
        _userAccount = userAccount;
        _validator = validator;
    }

    public async Task Handle(DeleteOwnerStaffCommand request, CancellationToken ct)
    {
        var vr = await _validator.ValidateAsync(request, ct);
        if (!vr.IsValid) throw new ValidationException(vr.Errors);

        if (!await _businessRepo.IsOwnedByAsync(request.BusinessId, _currentUser.UserId, ct))
            throw new ForbiddenException("You can't manage this business.");

        var staff = await _staffRepo.GetTrackedByBusinessAndIdAsync(request.BusinessId, request.StaffId, ct)
            ?? throw new NotFoundException("Staff", request.StaffId.ToString());

        
        var hasFuture = await _reservationRepo.HasFutureForStaffAsync(staff.Id, DateTimeOffset.UtcNow, ct);
        if (hasFuture)
            throw new ConflictException(
                "Cannot delete worker, they have upcoming bookings that are not yet completed" +
                "You need to cancel these bookings or assign other workers first"
            );

        // 1) delete staff entity
        await _staffRepo.DeleteAsync(staff, ct);
        await _staffRepo.SaveChangesAsync(ct);

        // 2) delete user account related to staff (if any)
        if (staff.UserId.HasValue)
            await _userAccount.DeleteUserAsync(staff.UserId.Value, ct);
    }
}
