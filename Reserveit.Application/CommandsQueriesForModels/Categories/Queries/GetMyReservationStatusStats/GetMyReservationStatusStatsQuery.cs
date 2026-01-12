using MediatR;
using Reserveit.Application.Common.DTOs.CategoryDtos;

namespace Reserveit.Application.CommandsQueriesForModels.Categories.Queries.GetMyReservationStatusStats;

public sealed record GetMyReservationStatusStatsQuery() : IRequest<ReservationStatusStatsDto>;