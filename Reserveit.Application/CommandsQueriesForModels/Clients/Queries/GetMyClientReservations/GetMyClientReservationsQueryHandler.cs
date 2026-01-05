using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Reserveit.Application.Common.DTOs.ReservationsDtos;
using Reserveit.Application.CurrentUserService;
using Reserveit.Domain.Interfaces;

namespace Reserveit.Application.CommandsQueriesForModels.Clients.Queries.GetMyClientReservations;

public sealed class GetMyClientReservationsQueryHandler
    : IRequestHandler<GetMyClientReservationsQuery, IReadOnlyList<ReservationDto>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IReservationRepository _reservationRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<GetMyClientReservationsQuery> _validator;
    private readonly ILogger<GetMyClientReservationsQueryHandler> _logger;

    public GetMyClientReservationsQueryHandler(
        ICurrentUser currentUser,
        IReservationRepository reservationRepository,
        IMapper mapper,
        IValidator<GetMyClientReservationsQuery> validator,
        ILogger<GetMyClientReservationsQueryHandler> logger)
    {
        _currentUser = currentUser;
        _reservationRepository = reservationRepository;
        _mapper = mapper;
        _validator = validator;
        _logger = logger;
    }

    public async Task<IReadOnlyList<ReservationDto>> Handle(GetMyClientReservationsQuery request, CancellationToken ct)
    {
        var vr = await _validator.ValidateAsync(request, ct);
        if (!vr.IsValid)
            throw new ValidationException(vr.Errors);

        var clientId = _currentUser.UserId;

        var reservations = await _reservationRepository.GetByClientIdAsync(
            clientId, request.Page, request.PageSize, ct);

        var dto = reservations.Select(_mapper.Map<ReservationDto>).ToList();

        _logger.LogInformation(
            "Client reservations fetched. ClientId={ClientId}, Page={Page}, PageSize={PageSize}, Count={Count}",
            clientId, request.Page, request.PageSize, dto.Count);

        return dto;
    }
}