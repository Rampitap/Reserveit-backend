using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reserveit.Application.CommandsQueriesForModels.Reservations.Commands.ChangeBusinessReservationStatus;
using Reserveit.Application.CommandsQueriesForModels.Reservations.Queries.GetBusinessReservations;
using Reserveit.Application.Common.DTOs.ReservationsDtos;
using Reserveit.Domain.Constants;
using Reserveit.Domain.Enums;

namespace Reserveit.API.Controllers;

[ApiController]
[Authorize(Roles = UserRoles.Owner)]
[Route("api/owner/business/{businessId:guid}/reservations")]
public sealed class OwnerReservationsController : ControllerBase
{
    private readonly IMediator _mediator;
    public OwnerReservationsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromRoute] Guid businessId,
        [FromQuery] DateTimeOffset from,
        [FromQuery] DateTimeOffset to,
        [FromQuery] ReservationStatus? status,
        [FromQuery] Guid? staffId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var query = new GetBusinessReservationsQuery(
            businessId,
            from,
            to,
            status,
            staffId,
            page,
            pageSize);

        return Ok(await _mediator.Send(query, ct));
    }

    [HttpPatch("{reservationId:guid}/status")]
    public async Task<IActionResult> ChangeStatus(
       [FromRoute] Guid businessId,
       [FromRoute] Guid reservationId,
       [FromBody] ChangeReservationStatusDto dto,
       CancellationToken ct = default)
    {
        await _mediator.Send(new ChangeBusinessReservationStatusCommand(businessId, reservationId, dto), ct);
        return NoContent();
    }


}
