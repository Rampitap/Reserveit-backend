using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reserveit.Application.Common.DTOs.AuthDtod;
using Reserveit.Application.Interfaces;
using System.Security.Authentication;


namespace Reserveit.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        // Валідація DTO відбувається автоматично (FluentValidation),
        // якщо є помилки - повернеться 400 ще до входу в метод.

        try
        {
            var result = await _authService.RegisterAsync(dto);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            var result = await _authService.LoginAsync(dto);
            return Ok(result);
        }
        catch (InvalidCredentialException)
        {
            return Unauthorized(new { message = "Invalid email or password" });
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync();
        return Ok(new { message = "Logged out successfully" });
    }

    [HttpGet("me")]
    [Authorize] 
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Me()
    {
        try
        {
            
            var result = await _authService.GetCurrentUserAsync(User);
            return Ok(result);
        }
        catch (InvalidCredentialException)
        {
            return Unauthorized();
        }
    }




}
