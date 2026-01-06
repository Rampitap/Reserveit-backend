using AutoMapper;
using MediatR;
using Reserveit.Application.Common.DTOs.BuisnessDtos;
using Reserveit.Domain.Exceptions;
using Reserveit.Domain.Interfaces;

namespace Reserveit.Application.CommandsQueriesForModels.Businesses.Queries.GetPublicBusiness;

public sealed class GetPublicBusinessDetailsQueryHandler
    : IRequestHandler<GetPublicBusinessDetailsQuery, PublicBusinessDetailsDto>
{
    private readonly IBusinessRepository _businessRepo;
    private readonly IMapper _mapper;

    public GetPublicBusinessDetailsQueryHandler(IBusinessRepository businessRepo, IMapper mapper)
    {
        _businessRepo = businessRepo;
        _mapper = mapper;
    }

    public async Task<PublicBusinessDetailsDto> Handle(GetPublicBusinessDetailsQuery request, CancellationToken ct)
    {
        var b = await _businessRepo.GetPublicByIdAsync(request.BusinessId, ct)
            ?? throw new NotFoundException("Business", request.BusinessId.ToString());

        return _mapper.Map<PublicBusinessDetailsDto>(b);
    }
}
