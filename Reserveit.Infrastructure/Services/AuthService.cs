using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Reserveit.Application.Common.DTOs.AuthDtod;
using Reserveit.Application.Interfaces;
using Reserveit.Domain.Constants;
using Reserveit.Domain.Entities;
using System.Security.Authentication;
using System.Security.Claims;

namespace Reserveit.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IMapper _mapper;
    private readonly IValidator<RegisterDto> _registerValidator;
    private readonly IValidator<LoginDto> _loginValidator;

    public AuthService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        IMapper mapper,
        IValidator<RegisterDto> registerValidator,
        IValidator<LoginDto> loginValidator)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _mapper = mapper;
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
    }


    public async Task<ResponseDto> RegisterAsync(RegisterDto dto)
    {
        var validationResult = await _registerValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new ArgumentException(errors);
        }

        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
        {
            throw new ArgumentException("User with this email already exists.");
        }

        
        if (dto.Role != UserRoles.Client && dto.Role != UserRoles.Owner)
        {
            throw new ArgumentException($"Registration allowed only for '{UserRoles.Client}' and '{UserRoles.Owner}'.");
        }

        
        if (!await _roleManager.RoleExistsAsync(dto.Role))
        {
            throw new ArgumentException($"Role '{dto.Role}' does not exist in the system.");
        }

        
        var user = _mapper.Map<User>(dto);

       
        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new ArgumentException($"Registration failed: {errors}");
        }

        // 5. Присвоєння ролі
        await _userManager.AddToRoleAsync(user, dto.Role);

        
        await _signInManager.SignInAsync(user, isPersistent: true);

        
        var response = _mapper.Map<ResponseDto>(user);
        response.Roles = await _userManager.GetRolesAsync(user); 

        return response;
    }

    public async Task<ResponseDto> LoginAsync(LoginDto dto)
    {
        var validationResult = await _loginValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new ArgumentException(errors);
        }


        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
        {
            // for security reasons, do not reveal whether the email or password was incorrect
            throw new InvalidCredentialException("Invalid email or password.");
        }

        // isPersistent: kukie lived for ExpireTimeSpan time (e.g., 7 days)
        var result = await _signInManager.PasswordSignInAsync(user, dto.Password, isPersistent: true, lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            throw new InvalidCredentialException("Invalid email or password.");
        }

        var response = _mapper.Map<ResponseDto>(user);
        response.Roles = await _userManager.GetRolesAsync(user);

        return response;
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<ResponseDto> GetCurrentUserAsync(ClaimsPrincipal principal)
    {
        // get user from ClaimsPrincipal
        var user = await _userManager.GetUserAsync(principal);

        if (user == null)
        {
            throw new InvalidCredentialException("User not found or session expired.");
        }

        var response = _mapper.Map<ResponseDto>(user);
        response.Roles = await _userManager.GetRolesAsync(user);

        return response;
    }
}
