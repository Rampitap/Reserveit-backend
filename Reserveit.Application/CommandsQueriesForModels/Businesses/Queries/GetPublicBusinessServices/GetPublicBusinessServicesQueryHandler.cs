using AutoMapper;
using MediatR;
using Reserveit.Application.Common.DTOs.ServiceDtos;
using Reserveit.Domain.Interfaces;

namespace Reserveit.Application.CommandsQueriesForModels.Businesses.Queries.GetPublicBusinessServices;

public sealed class GetPublicBusinessServicesQueryHandler
    : IRequestHandler<GetPublicBusinessServicesQuery, IReadOnlyList<PublicServiceDto>>
{
    private readonly IBusinessRepository _repo;
    private readonly IMapper _mapper;

    public GetPublicBusinessServicesQueryHandler(IBusinessRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<PublicServiceDto>> Handle(GetPublicBusinessServicesQuery request, CancellationToken ct)
    {
        var services = await _repo.GetPublicServicesAsync(request.BusinessId, ct);
        return services.Select(_mapper.Map<PublicServiceDto>).ToList();
    }
}
