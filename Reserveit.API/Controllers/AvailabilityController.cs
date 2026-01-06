using MediatR;
using Microsoft.AspNetCore.Mvc;
using Reserveit.Application.CommandsQueriesForModels.Businesses.Queries.GetAvailableSlots;

namespace Reserveit.API.Controllers;

[ApiController]
[Route("api/business")]
public sealed class AvailabilityController : ControllerBase
{
    private readonly IMediator _mediator;
    public AvailabilityController(IMediator mediator) => _mediator = mediator;

    // GET /api/business/{businessId}/staff/{staffId}/available-slots?serviceId=...&date=2026-01-10&stepMinutes=15
    [HttpGet("{businessId:guid}/staff/{staffId:guid}/available-slots")]
    public async Task<IActionResult> GetAvailableSlots(
        [FromRoute] Guid businessId,
        [FromRoute] Guid staffId,
        [FromQuery] Guid serviceId,
        [FromQuery] DateOnly date,
        [FromQuery] int stepMinutes = 15)
    {
        var dto = await _mediator.Send(new GetAvailableSlotsQuery(businessId, staffId, serviceId, date, stepMinutes));
        return Ok(dto);
    }
}