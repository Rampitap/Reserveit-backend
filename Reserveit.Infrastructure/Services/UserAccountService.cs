using Microsoft.AspNetCore.Identity;
using Reserveit.Application.Interfaces;
using Reserveit.Domain.Entities;
using Reserveit.Domain.Exceptions;

namespace Reserveit.Infrastructure.Services;

public sealed class UserAccountService : IUserAccountService
{
    private readonly UserManager<User> _userManager;

    public UserAccountService(UserManager<User> userManager) => _userManager = userManager;

    public async Task SetIsActiveAsync(Guid userId, bool isActive, CancellationToken ct)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString())
            ?? throw new NotFoundException("User", userId.ToString());

        user.IsActive = isActive;
        var res = await _userManager.UpdateAsync(user);
        if (!res.Succeeded)
            throw new InvalidOperationException(string.Join("; ", res.Errors.Select(e => e.Description)));
    }

    public async Task DeleteUserAsync(Guid userId, CancellationToken ct)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString())
            ?? throw new NotFoundException("User", userId.ToString());

        var res = await _userManager.DeleteAsync(user);
        if (!res.Succeeded)
            throw new InvalidOperationException(string.Join("; ", res.Errors.Select(e => e.Description)));
    }
}