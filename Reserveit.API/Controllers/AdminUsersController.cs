using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reserveit.Application.CommandsQueriesForModels.AdminManagement.Commands.DeleteAdminUser;
using Reserveit.Application.CommandsQueriesForModels.AdminManagement.Commands.UpdateAdminUserStatus;
using Reserveit.Application.CommandsQueriesForModels.AdminManagement.Queries.GetAdminUserById;
using Reserveit.Application.CommandsQueriesForModels.AdminManagement.Queries.GetAdminUsers;
using Reserveit.Application.Common.DTOs.AdminManageDtos;
using Reserveit.Domain.Constants;

namespace Reserveit.API.Controllers;

[ApiController]
[Route("api/admin/users")]
[Authorize(Roles = UserRoles.Admin)]
public sealed class AdminUsersController : ControllerBase
{
    private readonly IMediator _mediator;
    public AdminUsersController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] GetAdminUsersQuery query, CancellationToken ct)
        => Ok(await _mediator.Send(query, ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetAdminUserByIdQuery(id), ct));

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus([FromRoute] Guid id, [FromBody] UpdateAdminUserStatusBody body, CancellationToken ct)
    {
        await _mediator.Send(new UpdateAdminUserStatusCommand(id, body.IsActive), ct);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteAdminUserCommand(id), ct);
        return NoContent();
    }
}
