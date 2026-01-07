using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reserveit.Application.CommandsQueriesForModels.Staffes.Commands.CreateStaffByOwner;
using Reserveit.Application.Common.DTOs.StaffDtos;
using Reserveit.Domain.Constants;

namespace Reserveit.API.Controllers;

[ApiController]
[Route("api/owner/staff")]
[Authorize(Roles = UserRoles.Owner)]
public sealed class OwnerStaffController : ControllerBase
{
    private readonly IMediator _mediator;
    public OwnerStaffController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStaffAccountDto body, CancellationToken ct)
    {
        var id = await _mediator.Send(new CreateStaffCommand(body), ct);
        return Created("", new { id });
    }
}
