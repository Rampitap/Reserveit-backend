using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Reserveit.Application.Common.DTOs.ReservationsDtos;
using Reserveit.Application.Common.Pagination;
using Reserveit.Application.CurrentUserService;
using Reserveit.Domain.Enums;
using Reserveit.Domain.Interfaces;

namespace Reserveit.Application.CommandsQueriesForModels.Staffes.Queries.GetMyStaffReservations;

public sealed class GetMyStaffReservationsQueryHandler
    : IRequestHandler<GetMyStaffReservationsQuery, PagedResult<ReservationDto>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IStaffRepository _staffRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IValidator<GetMyStaffReservationsQuery> _validator;
    private readonly IMapper _mapper;
    private readonly ILogger<GetMyStaffReservationsQueryHandler> _logger;

    public GetMyStaffReservationsQueryHandler(
        ICurrentUser currentUser,
        IStaffRepository staffRepository,
        IReservationRepository reservationRepository,
        IValidator<GetMyStaffReservationsQuery> validator,
        IMapper mapper,
        ILogger<GetMyStaffReservationsQueryHandler> logger)
    {
        _currentUser = currentUser;
        _staffRepository = staffRepository;
        _reservationRepository = reservationRepository;
        _validator = validator;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedResult<ReservationDto>> Handle(GetMyStaffReservationsQuery request, CancellationToken ct)
    {
        var vr = await _validator.ValidateAsync(request, ct);
        if (!vr.IsValid)
            throw new ValidationException(vr.Errors);

        var userId = _currentUser.UserId;

        var staff = await _staffRepository.GetByUserIdAsync(userId, ct);
        if (staff is null)
        {
            _logger.LogWarning("Staff profile not found for user. UserId={UserId}", userId);
            throw new UnauthorizedAccessException("Staff profile not found for this user");
        }

        var page = request.Page < 1 ? 1 : request.Page;
        var pageSize = request.PageSize < 1 ? 12 : request.PageSize;

        // items 
        var items = await _reservationRepository.GetForStaffRangeAsync(
            staff.Id,
            request.From,
            request.To,
            request.Status,
            page,
            pageSize,
            ct);

        // total 
        var total = await _reservationRepository.CountForStaffRangeAsync(
            staff.Id,
            request.From,
            request.To,
            request.Status,
            ct);

        // 
        items = request.Sort switch
        {
            ReservationSort.StartDesc =>
                items.OrderByDescending(r => r.StartAt).ToList(),

            ReservationSort.StatusThenStart =>
                items.OrderBy(r => r.Status == ReservationStatus.Confirmed ? 0 :
                                   r.Status == ReservationStatus.Pending ? 1 : 2)
                     .ThenBy(r => r.StartAt)
                     .ToList(),

            _ =>
                items.OrderBy(r => r.StartAt).ToList()
        };

        var dto = items.Select(_mapper.Map<ReservationDto>).ToList();

        _logger.LogInformation(
            "Staff reservations loaded. StaffId={StaffId}, From={From}, To={To}, Page={Page}, PageSize={PageSize}, Total={Total}",
            staff.Id, request.From, request.To, page, pageSize, total);

        return new PagedResult<ReservationDto>
        {
            Page = page,
            PageSize = pageSize,
            Total = total,
            Items = dto
        };
    }
}