using AutoMapper;
using FluentValidation;
using MediatR;
using Reserveit.Application.Common.DTOs.StaffDtos;
using Reserveit.Application.CurrentUserService;
using Reserveit.Domain.Exceptions;
using Reserveit.Domain.Interfaces;

namespace Reserveit.Application.CommandsQueriesForModels.Staffes.Queries.GetOwnerBusinessStaff;

public sealed class GetOwnerBusinessStaffQueryHandler
    : IRequestHandler<GetOwnerBusinessStaffQuery, IReadOnlyList<OwnerStaffDto>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IBusinessRepository _businessRepository;
    private readonly IStaffRepository _staffRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<GetOwnerBusinessStaffQuery> _validator;

    public GetOwnerBusinessStaffQueryHandler(
        ICurrentUser currentUser,
        IBusinessRepository businessRepository,
        IStaffRepository staffRepository,
        IMapper mapper,
        IValidator<GetOwnerBusinessStaffQuery> validator)
    {
        _currentUser = currentUser;
        _businessRepository = businessRepository;
        _staffRepository = staffRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<IReadOnlyList<OwnerStaffDto>> Handle(GetOwnerBusinessStaffQuery request, CancellationToken ct)
    {
        var vr = await _validator.ValidateAsync(request, ct);
        if (!vr.IsValid) throw new ValidationException(vr.Errors);

        var ownerId = _currentUser.UserId;

        if (!await _businessRepository.IsOwnedByAsync(request.BusinessId, ownerId, ct))
            throw new ForbiddenException("You can't manage this business.");

        var staff = await _staffRepository.GetByBusinessIdAsync(request.BusinessId, ct);
        return _mapper.Map<List<OwnerStaffDto>>(staff);
    }
}
