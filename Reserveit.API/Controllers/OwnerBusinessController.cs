using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reserveit.Application.CommandsQueriesForModels.Businesses.Queries.GetOwnerBusinesses;
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
}
