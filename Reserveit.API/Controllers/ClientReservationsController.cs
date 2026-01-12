using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reserveit.Application.CommandsQueriesForModels.Categories.Queries.GetMyReservationStatusStats;
using Reserveit.Application.CommandsQueriesForModels.Clients.Commands.CancelClientReservation;
using Reserveit.Application.CommandsQueriesForModels.Clients.Commands.CreateReservation;
using Reserveit.Application.CommandsQueriesForModels.Clients.Queries.GetMyClientReservations;
using Reserveit.Application.Common.DTOs.ReservationsDtos;
using Reserveit.Domain.Constants;

namespace Reserveit.API.Controllers;

[ApiController]
[Route("api/client/reservations")]
[Authorize(Roles = UserRoles.Client)]
public sealed class ClientReservationsController : ControllerBase
{
    private readonly IMediator _mediator;
    public ClientReservationsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReservationDto body)
    {
        var id = await _mediator.Send(new CreateClientReservationCommand(body));
        return Created("", new { id }); // без CreatedAtAction поки
    }

    [HttpGet]
    public async Task<IActionResult> GetMine([FromQuery] GetMyClientReservationsQuery query)
    => Ok(await _mediator.Send(query));


    [HttpPatch("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel([FromRoute] Guid id)
    {
        await _mediator.Send(new CancelClientReservationCommand(id));
        return NoContent();
    }

    [HttpGet("my/stats")]
    public async Task<IActionResult> GetMyStats(CancellationToken ct)
    => Ok(await _mediator.Send(new GetMyReservationStatusStatsQuery(), ct));
}