using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Reserveit.Application.Common.DTOs.ReservationsDtos;
using Reserveit.Application.Common.Pagination;
using Reserveit.Application.CurrentUserService;
using Reserveit.Domain.Interfaces;

namespace Reserveit.Application.CommandsQueriesForModels.Clients.Queries.GetMyClientReservations;

public sealed class GetMyClientReservationsQueryHandler
    : IRequestHandler<GetMyClientReservationsQuery, PagedResult<ReservationDto>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IReservationRepository _reservationRepository;
    private readonly IValidator<GetMyClientReservationsQuery> _validator;
    private readonly IMapper _mapper;
    private readonly ILogger<GetMyClientReservationsQueryHandler> _logger;

    public GetMyClientReservationsQueryHandler(
        ICurrentUser currentUser,
        IReservationRepository reservationRepository,
        IValidator<GetMyClientReservationsQuery> validator,
        IMapper mapper,
        ILogger<GetMyClientReservationsQueryHandler> logger)
    {
        _currentUser = currentUser;
        _reservationRepository = reservationRepository;
        _validator = validator;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedResult<ReservationDto>> Handle(GetMyClientReservationsQuery request, CancellationToken ct)
    {
        var vr = await _validator.ValidateAsync(request, ct);
        if (!vr.IsValid) throw new ValidationException(vr.Errors);

        var page = request.Page < 1 ? 1 : request.Page;
        var pageSize = request.PageSize < 1 ? 12 : request.PageSize;

        var clientId = _currentUser.UserId;

        // items
        var items = await _reservationRepository.GetByClientIdAsync(clientId, page, pageSize, ct);

        // total (status можеш додати в repo-count якщо треба фільтр)
        var total = await _reservationRepository.CountByClientIdAsync(clientId, request.Status, ct);

        _logger.LogInformation(
            "Client reservations fetched. ClientId={ClientId}, Page={Page}, PageSize={PageSize}, Total={Total}",
            clientId, page, pageSize, total);

        return new PagedResult<ReservationDto>
        {
            Page = page,
            PageSize = pageSize,
            Total = total,
            Items = items.Select(_mapper.Map<ReservationDto>).ToList()
        };
    }
}