using MediatR;
using Microsoft.AspNetCore.Mvc;
using Reserveit.Application.CommandsQueriesForModels.Businesses.Queries.GetAllPublicBusinesses;
using Reserveit.Application.CommandsQueriesForModels.Businesses.Queries.GetPublicBusiness;
using Reserveit.Application.CommandsQueriesForModels.Businesses.Queries.GetPublicBusinessServices;
using Reserveit.Application.CommandsQueriesForModels.Businesses.Queries.GetPublicBusinessStaff;

namespace Reserveit.API.Controllers;

[ApiController]
[Route("api/business")]
public sealed class BusinessController : ControllerBase
{
    private readonly IMediator _mediator;
    public BusinessController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBusinessDetails([FromRoute] Guid id)
        => Ok(await _mediator.Send(new GetPublicBusinessDetailsQuery(id)));

    [HttpGet("{id:guid}/services")]
    public async Task<IActionResult> GetBusinessServices([FromRoute] Guid id)
        => Ok(await _mediator.Send(new GetPublicBusinessServicesQuery(id)));

    [HttpGet("{id:guid}/staff")]
    public async Task<IActionResult> GetBusinessStaff([FromRoute] Guid id)
        => Ok(await _mediator.Send(new GetPublicBusinessStaffQuery(id)));

    [HttpGet("all")]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 12, [FromQuery] string? q = null)
    => Ok(await _mediator.Send(new GetPublicBusinessesQuery(page, pageSize, q)));
}
