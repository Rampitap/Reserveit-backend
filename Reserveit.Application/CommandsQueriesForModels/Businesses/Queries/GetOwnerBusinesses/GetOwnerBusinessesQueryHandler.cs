using AutoMapper;
using MediatR;
using Reserveit.Application.Common.DTOs.BuisnessDtos;
using Reserveit.Application.CurrentUserService;
using Reserveit.Domain.Interfaces;

namespace Reserveit.Application.CommandsQueriesForModels.Businesses.Queries.GetOwnerBusinesses;

public sealed class GetOwnerBusinessesQueryHandler
    : IRequestHandler<GetOwnerBusinessesQuery, IReadOnlyList<OwnerBusinessSummaryDto>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IBusinessRepository _businessRepository;
    private readonly IMapper _mapper;

    public GetOwnerBusinessesQueryHandler(
        ICurrentUser currentUser,
        IBusinessRepository businessRepository,
        IMapper mapper)
    {
        _currentUser = currentUser;
        _businessRepository = businessRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<OwnerBusinessSummaryDto>> Handle(GetOwnerBusinessesQuery request, CancellationToken ct)
    {
        var ownerId = _currentUser.UserId;
        var businesses = await _businessRepository.GetByOwnerIdAsync(ownerId, ct);
        return _mapper.Map<List<OwnerBusinessSummaryDto>>(businesses);
    }
}
