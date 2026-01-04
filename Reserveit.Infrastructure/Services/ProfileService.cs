using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Reserveit.Application.Common.DTOs.UserDtos;
using Reserveit.Application.Interfaces;
using Reserveit.Domain.Entities;

namespace Reserveit.Infrastructure.Services;

public class ProfileService : IProfileService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<ProfileService> _logger;

    public ProfileService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ILogger<ProfileService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    public async Task ChangePasswordAsync(Guid userId, ChangePasswordRequestDto data, CancellationToken ct)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            _logger.LogWarning("ChangePassword: user not found. UserId={UserId}", userId);
            throw new UnauthorizedAccessException("Користувач не знайдений.");
        }

        var result = await _userManager.ChangePasswordAsync(user, data.CurrentPassword, data.NewPassword);
        if (!result.Succeeded)
        {
            _logger.LogWarning("ChangePassword failed. UserId={UserId}. Codes={Codes}",
                userId, string.Join(",", result.Errors.Select(e => e.Code)));

            throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));
        }

        await _signInManager.RefreshSignInAsync(user);
        _logger.LogInformation("Password changed. UserId={UserId}", userId);
    }

    public async Task DeleteAccountAsync(Guid userId, DeleteMyAccountRequestDto data, CancellationToken ct)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            _logger.LogWarning("DeleteAccount: user not found. UserId={UserId}", userId);
            throw new UnauthorizedAccessException("User not found.");
        }

        var ok = await _userManager.CheckPasswordAsync(user, data.Password);
        if (!ok)
        {
            _logger.LogWarning("DeleteAccount failed: invalid password. UserId={UserId}", userId);
            throw new InvalidOperationException("Invalid password.");
        }

        await _signInManager.SignOutAsync();

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            _logger.LogWarning("DeleteAccount failed. UserId={UserId}. Codes={Codes}",
                userId, string.Join(",", result.Errors.Select(e => e.Code)));

            throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));
        }

        _logger.LogInformation("Account deleted. UserId={UserId}", userId);
    }
}
