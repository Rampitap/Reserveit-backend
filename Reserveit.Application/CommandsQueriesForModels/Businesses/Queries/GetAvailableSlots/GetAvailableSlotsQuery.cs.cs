using MediatR;
using Reserveit.Application.Common.DTOs.AvailabilityDtos;

namespace Reserveit.Application.CommandsQueriesForModels.Businesses.Queries.GetAvailableSlots;

public sealed record GetAvailableSlotsQuery(
    Guid BusinessId,
    Guid StaffId,
    Guid ServiceId,
    DateOnly Date,
    int StepMinutes = 15
) : IRequest<AvailabilitySlotsResponseDto>;
