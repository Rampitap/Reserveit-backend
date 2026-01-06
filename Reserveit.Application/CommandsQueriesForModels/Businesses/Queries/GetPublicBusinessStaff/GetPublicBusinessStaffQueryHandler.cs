using AutoMapper;
using MediatR;
using Reserveit.Application.Common.DTOs.StaffDtos;
using Reserveit.Domain.Interfaces;

namespace Reserveit.Application.CommandsQueriesForModels.Businesses.Queries.GetPublicBusinessStaff;

public sealed class GetPublicBusinessStaffQueryHandler
    : IRequestHandler<GetPublicBusinessStaffQuery, IReadOnlyList<PublicStaffDto>>
{
    private readonly IBusinessRepository _repo;
    private readonly IMapper _mapper;

    public GetPublicBusinessStaffQueryHandler(IBusinessRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<PublicStaffDto>> Handle(GetPublicBusinessStaffQuery request, CancellationToken ct)
    {
        var staff = await _repo.GetPublicStaffAsync(request.BusinessId, ct);
        return staff.Select(_mapper.Map<PublicStaffDto>).ToList();
    }
}
