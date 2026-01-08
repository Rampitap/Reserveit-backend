using AutoMapper;
using FluentValidation;
using MediatR;
using Reserveit.Application.Common.DTOs.StaffDtos;
using Reserveit.Application.CurrentUserService;
using Reserveit.Domain.Exceptions;
using Reserveit.Domain.Interfaces;

namespace Reserveit.Application.CommandsQueriesForModels.Staffes.Queries.GetOwnerBusinessStaffById;

public sealed class GetOwnerBusinessStaffByIdQueryHandler
    : IRequestHandler<GetOwnerBusinessStaffByIdQuery, OwnerStaffDto>
{
    private readonly ICurrentUser _currentUser;
    private readonly IBusinessRepository _businessRepository;
    private readonly IStaffRepository _staffRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<GetOwnerBusinessStaffByIdQuery> _validator;

    public GetOwnerBusinessStaffByIdQueryHandler(
        ICurrentUser currentUser,
        IBusinessRepository businessRepository,
        IStaffRepository staffRepository,
        IMapper mapper,
        IValidator<GetOwnerBusinessStaffByIdQuery> validator)
    {
        _currentUser = currentUser;
        _businessRepository = businessRepository;
        _staffRepository = staffRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<OwnerStaffDto> Handle(GetOwnerBusinessStaffByIdQuery request, CancellationToken ct)
    {
        var vr = await _validator.ValidateAsync(request, ct);
        if (!vr.IsValid) throw new ValidationException(vr.Errors);

        var ownerId = _currentUser.UserId;

        if (!await _businessRepository.IsOwnedByAsync(request.BusinessId, ownerId, ct))
            throw new ForbiddenException("You can't manage this business.");

        var staff = await _staffRepository.GetByBusinessAndIdAsync(request.BusinessId, request.StaffId, ct)
            ?? throw new NotFoundException("Staff", request.StaffId.ToString());

        return _mapper.Map<OwnerStaffDto>(staff);
    }
}
