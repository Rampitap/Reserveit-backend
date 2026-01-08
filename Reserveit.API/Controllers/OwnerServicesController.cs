using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reserveit.Application.CommandsQueriesForModels.Services.Commands.AddStaffToService;
using Reserveit.Application.CommandsQueriesForModels.Services.Commands.CreateOwnerService;
using Reserveit.Application.CommandsQueriesForModels.Services.Commands.DeactivateOwnerService;
using Reserveit.Application.CommandsQueriesForModels.Services.Commands.RemoveStaffFromService;
using Reserveit.Application.CommandsQueriesForModels.Services.Commands.UpdateOwnerService;
using Reserveit.Application.CommandsQueriesForModels.Services.Queries.GetOwnerServices;
using Reserveit.Application.Common.DTOs.ServiceDtos;
using Reserveit.Domain.Constants;

namespace Reserveit.API.Controllers;

[ApiController]
[Authorize(Roles = UserRoles.Owner)]
[Route("api/owner/business/{businessId:guid}/services")]
public sealed class OwnerServicesController : ControllerBase
{
    private readonly IMediator _mediator;
    public OwnerServicesController(IMediator mediator) => _mediator = mediator;

    // LIST
    [HttpGet]
    public async Task<IActionResult> GetList([FromRoute] Guid businessId, CancellationToken ct)
        => Ok(await _mediator.Send(new GetOwnerServicesQuery(businessId), ct));

    // CREATE
    [HttpPost]
    public async Task<IActionResult> Create([FromRoute] Guid businessId, [FromBody] CreateServiceDto body, CancellationToken ct)
    {
        var id = await _mediator.Send(new CreateOwnerServiceCommand(businessId, body), ct);
        return Created("", new { id });
    }

    // UPDATE
    [HttpPut("{serviceId:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid businessId, [FromRoute] Guid serviceId, [FromBody] UpdateServiceDto body, CancellationToken ct)
    {
        await _mediator.Send(new UpdateOwnerServiceCommand(businessId, serviceId, body), ct);
        return NoContent();
    }

    // DEACTIVATE
    [HttpDelete("{serviceId:guid}")]
    public async Task<IActionResult> Deactivate([FromRoute] Guid businessId, [FromRoute] Guid serviceId, CancellationToken ct)
    {
        await _mediator.Send(new DeactivateOwnerServiceCommand(businessId, serviceId), ct);
        return NoContent();
    }

    // ADD STAFF
    [HttpPost("{serviceId:guid}/staff/{staffId:guid}")]
    public async Task<IActionResult> AddStaff([FromRoute] Guid businessId, [FromRoute] Guid serviceId, [FromRoute] Guid staffId, CancellationToken ct)
    {
        await _mediator.Send(new AddStaffToServiceCommand(businessId, serviceId, staffId), ct);
        return NoContent();
    }

    // REMOVE STAFF
    [HttpDelete("{serviceId:guid}/staff/{staffId:guid}")]
    public async Task<IActionResult> RemoveStaff([FromRoute] Guid businessId, [FromRoute] Guid serviceId, [FromRoute] Guid staffId, CancellationToken ct)
    {
        await _mediator.Send(new RemoveStaffFromServiceCommand(businessId, serviceId, staffId), ct);
        return NoContent();
    }
}
