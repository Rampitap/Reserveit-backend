using MediatR;
using Reserveit.Application.Common.DTOs.AvailabilityDtos;
using Reserveit.Application.Extensions;
using Reserveit.Domain.Entities;
using Reserveit.Domain.Exceptions;
using Reserveit.Domain.Interfaces;

namespace Reserveit.Application.CommandsQueriesForModels.Businesses.Queries.GetAvailableSlots;

public sealed class GetAvailableSlotsQueryHandler
    : IRequestHandler<GetAvailableSlotsQuery, AvailabilitySlotsResponseDto>
{
    private readonly IBusinessRepository _businessRepository;
    private readonly IStaffRepository _staffRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IReservationRepository _reservationRepository;

    public GetAvailableSlotsQueryHandler(
        IBusinessRepository businessRepository,
        IStaffRepository staffRepository,
        IServiceRepository serviceRepository,
        IReservationRepository reservationRepository)
    {
        _businessRepository = businessRepository;
        _staffRepository = staffRepository;
        _serviceRepository = serviceRepository;
        _reservationRepository = reservationRepository;
    }

    public async Task<AvailabilitySlotsResponseDto> Handle(GetAvailableSlotsQuery request, CancellationToken ct)
    {
        if (request.StepMinutes <= 0 || request.StepMinutes > 60)
            throw new InvalidOperationException("StepMinutes has to be in range 1..60.");

        var business = await _businessRepository.GetPublicByIdAsync(request.BusinessId, ct)
            ?? throw new NotFoundException(nameof(Business), request.BusinessId.ToString());

        if (business.OpeningTime is null || business.ClosingTime is null)
            throw new InvalidOperationException("You should input open hours for business (OpeningTime/ClosingTime).");

        var service = await _serviceRepository.GetByIdAsync(request.ServiceId, ct)
            ?? throw new NotFoundException("Service", request.ServiceId.ToString());

        if (!service.IsActive)
            throw new InvalidOperationException("Service is inactive.");

        if (service.BusinessId != business.Id)
            throw new InvalidOperationException("Service doesn't belong to this business.");

        var staff = await _staffRepository.GetByIdAsync(request.StaffId, ct)
            ?? throw new NotFoundException("Staff", request.StaffId.ToString());

        if (!staff.IsActive)
            throw new InvalidOperationException("Staff is inactive.");

        if (staff.BusinessId != business.Id)
            throw new InvalidOperationException("The staff doesn't belong to this business.");

        var tz = TimeZoneHelper.Resolve(business.Timezone);

        // DateOnly.ToDateTime needs TimeOnly, not TimeSpan
        var localDate = request.Date;

        var openLocal = localDate.ToDateTime(TimeOnly.FromTimeSpan(business.OpeningTime.Value));
        var closeLocal = localDate.ToDateTime(TimeOnly.FromTimeSpan(business.ClosingTime.Value));

        if (closeLocal <= openLocal)
            throw new InvalidOperationException("ClosingTime has to be later than OpeningTime.");

        var fromUtc = TimeZoneHelper.LocalToUtc(openLocal, tz);
        var toUtc = TimeZoneHelper.LocalToUtc(closeLocal, tz);

        var busy = await _reservationRepository.GetBlockingForStaffAsync(request.StaffId, fromUtc, toUtc, ct);

        var duration = TimeSpan.FromMinutes(service.DurationMinutes);
        var step = TimeSpan.FromMinutes(request.StepMinutes);

        var result = new AvailabilitySlotsResponseDto
        {
            BusinessId = business.Id,
            StaffId = staff.Id,
            ServiceId = service.Id,
            Date = request.Date.ToString("yyyy-MM-dd"),
            Timezone = business.Timezone,
            DurationMinutes = service.DurationMinutes,
            StepMinutes = request.StepMinutes,
            OpeningTime = business.OpeningTime.Value.ToString(@"hh\:mm"),
            ClosingTime = business.ClosingTime.Value.ToString(@"hh\:mm"),
        };

        for (var cursorLocal = openLocal; cursorLocal + duration <= closeLocal; cursorLocal = cursorLocal.Add(step))
        {
            var cursorUtc = TimeZoneHelper.LocalToUtc(cursorLocal, tz);

            // slots in the past are not available
            if (cursorUtc <= DateTimeOffset.UtcNow)
                continue;

            var endUtc = cursorUtc + duration;

            var overlaps = busy.Any(r => r.StartAt < endUtc && r.EndAt > cursorUtc);
            if (!overlaps)
            {
                // return time with correct business offset
                result.AvailableStartTimes.Add(TimeZoneHelper.LocalToOffset(cursorLocal, tz));
            }
        }

        return result;
    }
}
