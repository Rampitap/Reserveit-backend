using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reserveit.Application.CommandsQueriesForModels.Staffes.Commands.ChangeReservationStatus;
using Reserveit.Application.CommandsQueriesForModels.Staffes.Queries.GetMyStaffReservations;
using Reserveit.Application.Common.DTOs.ReservationsDtos;
using Reserveit.Domain.Constants;

namespace Reserveit.API.Controllers;

[ApiController]
[Route("api/staff/reservations")]
[Authorize(Roles = UserRoles.Staff)]
public sealed class StaffReservationsController : ControllerBase
{
    private readonly IMediator _mediator;
    public StaffReservationsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetMyStaffReservationsQuery query)
        => Ok(await _mediator.Send(query));

    [HttpPatch("{reservationId:guid}/status")]
    public async Task<IActionResult> ChangeStatus([FromRoute] Guid reservationId, [FromBody] ChangeReservationStatusDto body)
    {
        await _mediator.Send(new ChangeStaffReservationStatusCommand(reservationId, body));
        return NoContent();
    }
}
