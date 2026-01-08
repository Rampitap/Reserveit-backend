using AutoMapper;
using FluentValidation;
using MediatR;
using Reserveit.Application.Common.DTOs.ServiceDtos;
using Reserveit.Application.CurrentUserService;
using Reserveit.Domain.Exceptions;
using Reserveit.Domain.Interfaces;

namespace Reserveit.Application.CommandsQueriesForModels.Services.Queries.GetOwnerServices;

public sealed class GetOwnerServicesQueryHandler
    : IRequestHandler<GetOwnerServicesQuery, IReadOnlyList<OwnerServiceDto>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IBusinessRepository _businessRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<GetOwnerServicesQuery> _validator;

    public GetOwnerServicesQueryHandler(
        ICurrentUser currentUser,
        IBusinessRepository businessRepository,
        IServiceRepository serviceRepository,
        IMapper mapper,
        IValidator<GetOwnerServicesQuery> validator)
    {
        _currentUser = currentUser;
        _businessRepository = businessRepository;
        _serviceRepository = serviceRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<IReadOnlyList<OwnerServiceDto>> Handle(GetOwnerServicesQuery request, CancellationToken ct)
    {
        var vr = await _validator.ValidateAsync(request, ct);
        if (!vr.IsValid) throw new ValidationException(vr.Errors);

        var ownerId = _currentUser.UserId;
        if (!await _businessRepository.IsOwnedByAsync(request.BusinessId, ownerId, ct))
            throw new ForbiddenException("You can't manage this business.");

        var services = await _serviceRepository.GetByBusinessIdAsync(request.BusinessId, ct);
        return _mapper.Map<List<OwnerServiceDto>>(services);
    }
}
