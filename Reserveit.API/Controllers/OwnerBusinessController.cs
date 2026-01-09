using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reserveit.Application.CommandsQueriesForModels.Businesses.Commands.CreateOwnerBusiness;
using Reserveit.Application.CommandsQueriesForModels.Businesses.Commands.DeleteOwnerBusiness;
using Reserveit.Application.CommandsQueriesForModels.Businesses.Commands.UpdateOwnerBusiness;
using Reserveit.Application.CommandsQueriesForModels.Businesses.Commands.UpdateOwnerBusinessStatus;
using Reserveit.Application.CommandsQueriesForModels.Businesses.Queries.GetOwnerBusinessById;
using Reserveit.Application.CommandsQueriesForModels.Businesses.Queries.GetOwnerBusinesses;
using Reserveit.Application.Common.DTOs.BuisnessDtos;
using Reserveit.Domain.Constants;

namespace Reserveit.API.Controllers;

[ApiController]
[Route("api/owner/business")]
[Authorize(Roles = UserRoles.Owner)]
public sealed class OwnerBusinessController : ControllerBase
{
    private readonly IMediator _mediator;
    public OwnerBusinessController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetMine(CancellationToken ct)
        => Ok(await _mediator.Send(new GetOwnerBusinessesQuery(), ct));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBusinessDto body, CancellationToken ct)
    {
        var id = await _mediator.Send(new CreateOwnerBusinessCommand(body), ct);
        return Created("", new { id });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetOwnerBusinessByIdQuery(id), ct));

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateBusinessDto body, CancellationToken ct)
    {
        await _mediator.Send(new UpdateOwnerBusinessCommand(id, body), ct);
        return NoContent();
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus([FromRoute] Guid id, [FromBody] UpdateBusinessStatusDto body, CancellationToken ct)
    {
        await _mediator.Send(new UpdateOwnerBusinessStatusCommand(id, body.IsActive), ct);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteOwnerBusinessCommand(id), ct);
        return NoContent();
    }
}
