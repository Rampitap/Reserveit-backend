using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Reserveit.Application.CurrentUserService;
using Reserveit.Domain.Exceptions;
using Reserveit.Domain.Interfaces;

namespace Reserveit.Application.CommandsQueriesForModels.Staffes.Commands.UpdateOwnerStaff;

public sealed class UpdateOwnerStaffCommandHandler : IRequestHandler<UpdateOwnerStaffCommand>
{
    private readonly ICurrentUser _currentUser;
    private readonly IBusinessRepository _businessRepository;
    private readonly IStaffRepository _staffRepository;
    private readonly IValidator<UpdateOwnerStaffCommand> _validator;
    private readonly ILogger<UpdateOwnerStaffCommandHandler> _logger;

    public UpdateOwnerStaffCommandHandler(
        ICurrentUser currentUser,
        IBusinessRepository businessRepository,
        IStaffRepository staffRepository,
        IValidator<UpdateOwnerStaffCommand> validator,
        ILogger<UpdateOwnerStaffCommandHandler> logger)
    {
        _currentUser = currentUser;
        _businessRepository = businessRepository;
        _staffRepository = staffRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task Handle(UpdateOwnerStaffCommand request, CancellationToken ct)
    {
        var vr = await _validator.ValidateAsync(request, ct);
        if (!vr.IsValid)
            throw new ValidationException(vr.Errors);

        var ownerId = _currentUser.UserId;

       
        var isOwned = await _businessRepository.IsOwnedByAsync(request.BusinessId, ownerId, ct);
        if (!isOwned)
        {
            _logger.LogWarning(
                "UpdateStaff forbidden: business not owned. OwnerId={OwnerId}, BusinessId={BusinessId}",
                ownerId, request.BusinessId);

            throw new ForbiddenException("You don't have access to this business.");
        }

        
        var staff = await _staffRepository.GetTrackedByBusinessAndIdAsync(request.BusinessId, request.StaffId, ct);
        if (staff is null)
        {
            _logger.LogWarning(
                "UpdateStaff: staff not found. BusinessId={BusinessId}, StaffId={StaffId}",
                request.BusinessId, request.StaffId);

            throw new NotFoundException("Staff", request.StaffId.ToString());
        }

        staff.DisplayName = request.Data.DisplayName.Trim();
        staff.Bio = string.IsNullOrWhiteSpace(request.Data.Bio) ? null : request.Data.Bio.Trim();
        staff.UpdatedAt = DateTimeOffset.UtcNow;

        await _staffRepository.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Owner updated staff. OwnerId={OwnerId}, BusinessId={BusinessId}, StaffId={StaffId}",
            ownerId, request.BusinessId, request.StaffId);
    }
}
