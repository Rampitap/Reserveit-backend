using MediatR;
using Reserveit.Application.Common.DTOs.CategoryDtos;
using Reserveit.Application.CurrentUserService;
using Reserveit.Domain.Entities;
using Reserveit.Domain.Enums;
using Reserveit.Domain.Interfaces;

namespace Reserveit.Application.CommandsQueriesForModels.Categories.Queries.GetMyReservationStatusStats;

public sealed class GetMyReservationStatusStatsQueryHandler
    : IRequestHandler<GetMyReservationStatusStatsQuery, ReservationStatusStatsDto>
{
    private readonly ICurrentUser _currentUser;
    private readonly ICategoryRepository _repo;

    public GetMyReservationStatusStatsQueryHandler(ICurrentUser currentUser, ICategoryRepository repo)
    {
        _currentUser = currentUser;
        _repo = repo;
    }

    public async Task<ReservationStatusStatsDto> Handle(GetMyReservationStatusStatsQuery request, CancellationToken ct) 
    {
        var dict = await _repo.GetClientStatusCountsAsync(_currentUser.UserId, ct);

        return new ReservationStatusStatsDto
        {
            Pending = dict.GetValueOrDefault(ReservationStatus.Pending),
            Confirmed = dict.GetValueOrDefault(ReservationStatus.Confirmed),
            Cancelled = dict.GetValueOrDefault(ReservationStatus.Cancelled),
            Completed = dict.GetValueOrDefault(ReservationStatus.Completed),
        };
    }
}
