using AutoMapper;
using FluentValidation;
using MediatR;
using Reserveit.Application.Common.DTOs.ReservationsDtos;
using Reserveit.Application.Common.Pagination;
using Reserveit.Application.CurrentUserService;
using Reserveit.Domain.Exceptions;
using Reserveit.Domain.Interfaces;

namespace Reserveit.Application.CommandsQueriesForModels.Reservations.Queries.GetBusinessReservations;

public sealed class GetBusinessReservationsQueryHandler
    : IRequestHandler<GetBusinessReservationsQuery, PagedResult<OwnerReservationDto>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IBusinessRepository _businessRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<GetBusinessReservationsQuery> _validator;

    public GetBusinessReservationsQueryHandler(
        ICurrentUser currentUser,
        IBusinessRepository businessRepository,
        IReservationRepository reservationRepository,
        IMapper mapper,
        IValidator<GetBusinessReservationsQuery> validator)
    {
        _currentUser = currentUser;
        _businessRepository = businessRepository;
        _reservationRepository = reservationRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<PagedResult<OwnerReservationDto>> Handle(GetBusinessReservationsQuery request, CancellationToken ct)
    {
        var vr = await _validator.ValidateAsync(request, ct);
        if (!vr.IsValid) throw new ValidationException(vr.Errors);

        var ownerId = _currentUser.UserId;

        if (!await _businessRepository.IsOwnedByAsync(request.BusinessId, ownerId, ct))
            throw new ForbiddenException("You can't manage this business.");

        var (items, total) = await _reservationRepository.GetForBusinessRangeAsync(
            request.BusinessId,
            request.From,
            request.To,
            request.Status,
            request.StaffId,
            request.Page,
            request.PageSize,
            ct);

        return new PagedResult<OwnerReservationDto>
        {
            Page = request.Page,
            PageSize = request.PageSize,
            Total = (int)total,
            Items = _mapper.Map<List<OwnerReservationDto>>(items)
        };
    }
}
