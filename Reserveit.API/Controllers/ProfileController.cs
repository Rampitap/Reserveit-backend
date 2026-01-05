using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reserveit.Application.Common.DTOs.UserDtos;
using Reserveit.Application.CommandsQueriesForModels.Profiles.Commands.DeleteMyProfile;
using Reserveit.Application.CommandsQueriesForModels.Profiles.Commands.UpdateMyPassword;
using Reserveit.Application.CommandsQueriesForModels.Profiles.Commands.UpdateMyProfile;
using Reserveit.Application.CommandsQueriesForModels.Profiles.Queries.GetMyProfile;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Reserveit.API.Controllers;

[ApiController]
[Route("api/profile")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProfileController(IMediator mediator) 
    {
        _mediator = mediator; 
        
    }


    [HttpGet]
    [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Get()
       => Ok(await _mediator.Send(new GetMyProfileQuery()));

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Update([FromBody] UpdateProfileDto body)
    {
        

        await _mediator.Send(new UpdateMyProfileCommand(body));
        return NoContent();
    }

    [HttpPut("password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto body)
    {
        

        await _mediator.Send(new UpdateMyPasswordCommand(body));
        return NoContent();
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete([FromBody] DeleteMyAccountRequestDto body)
    {
        

        await _mediator.Send(new DeleteMyProfileCommand(body));
        return NoContent();
    }
}
