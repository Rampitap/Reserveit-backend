using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reserveit.Application.CommandsQueriesForModels.Staffes.Commands.CreateStaffByOwner;
using Reserveit.Application.CommandsQueriesForModels.Staffes.Commands.DeleteOwnerStaff;
using Reserveit.Application.CommandsQueriesForModels.Staffes.Commands.UpdateOwnerStaff;
using Reserveit.Application.CommandsQueriesForModels.Staffes.Commands.UpdateOwnerStaffStatus;
using Reserveit.Application.CommandsQueriesForModels.Staffes.Queries.GetOwnerBusinessStaff;
using Reserveit.Application.CommandsQueriesForModels.Staffes.Queries.GetOwnerBusinessStaffById;
using Reserveit.Application.Common.DTOs.StaffDtos;
using Reserveit.Domain.Constants;

namespace Reserveit.API.Controllers;

[ApiController]
[Route("api/owner")]
[Authorize(Roles = UserRoles.Owner)]
public sealed class OwnerStaffController : ControllerBase
{
    private readonly IMediator _mediator;
    public OwnerStaffController(IMediator mediator) => _mediator = mediator;

    [HttpPost("staff")]
    public async Task<IActionResult> Create([FromBody] CreateStaffAccountDto body, CancellationToken ct)
    {
        var id = await _mediator.Send(new CreateStaffCommand(body), ct);
        return Created("", new { id });
    }



    [HttpGet("business/{businessId:guid}/staff")]
    public async Task<IActionResult> GetList([FromRoute] Guid businessId, CancellationToken ct)
       => Ok(await _mediator.Send(new GetOwnerBusinessStaffQuery(businessId), ct));



    [HttpGet("business/{businessId:guid}/staff/{staffId:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid businessId, [FromRoute] Guid staffId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetOwnerBusinessStaffByIdQuery(businessId, staffId), ct));



    [HttpPatch("business/{businessId:guid}/staff/{staffId:guid}/status")]
    public async Task<IActionResult> SetStatus(
    [FromRoute] Guid businessId,
    [FromRoute] Guid staffId,
    [FromBody] UpdateStaffStatusDto body,
    CancellationToken ct)
    {
        await _mediator.Send(new UpdateOwnerStaffStatusCommand(businessId, staffId, body), ct);
        return NoContent();
    }

    [HttpDelete("business/{businessId:guid}/staff/{staffId:guid}")]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid businessId,
        [FromRoute] Guid staffId,
        CancellationToken ct)
    {
        await _mediator.Send(new DeleteOwnerStaffCommand(businessId, staffId), ct);
        return NoContent();
    }

    [HttpPut("business/{businessId:guid}/staff/{staffId:guid}")]
    public async Task<IActionResult> Update(
    [FromRoute] Guid businessId,
    [FromRoute] Guid staffId,
    [FromBody] UpdateOwnerStaffDto body,
    CancellationToken ct)
    {
        await _mediator.Send(new UpdateOwnerStaffCommand(businessId, staffId, body), ct);
        return NoContent();
    }
}
