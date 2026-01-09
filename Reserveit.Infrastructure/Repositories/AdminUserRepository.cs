using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Reserveit.Application.Common.DTOs.AdminManageDtos;
using Reserveit.Application.Common.Pagination;
using Reserveit.Application.Interfaces;
using Reserveit.Domain.Entities;
using Reserveit.Domain.Exceptions;

namespace Reserveit.Infrastructure.Repositories;

public sealed class AdminUserRepository : IAdminUserRepository
{
    private readonly UserManager<User> _userManager;

    public AdminUserRepository(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<PagedResult<AdminUserDto>> GetPagedAsync(int page, int pageSize, string? q, CancellationToken ct)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 20 : pageSize;

        var query = _userManager.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(q))
        {
            var term = q.Trim().ToLower();
            query = query.Where(u =>
                (u.Email ?? "").ToLower().Contains(term) ||
                (u.FirstName ?? "").ToLower().Contains(term) ||
                (u.LastName ?? "").ToLower().Contains(term));
        }

        var total = await query.CountAsync(ct);

        var users = await query
            .OrderByDescending(u => u.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var items = new List<AdminUserDto>(users.Count);
        foreach (var u in users)
        {
            var roles = await _userManager.GetRolesAsync(u);

            items.Add(new AdminUserDto
            {
                Id = u.Id,
                Email = u.Email ?? "",
                FirstName = u.FirstName ?? "",
                LastName = u.LastName ?? "",
                IsActive = u.IsActive,
                BusinessId = u.BusinessId,
                Roles = roles.ToList(),
                CreatedAt = u.CreatedAt
            });
        }

        return new PagedResult<AdminUserDto>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            Total = total
        };
    }

    public async Task<AdminUserDto> GetByIdAsync(Guid userId, CancellationToken ct)
    {
        var u = await _userManager.Users.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == userId, ct)
            ?? throw new NotFoundException("User", userId.ToString());

        var roles = await _userManager.GetRolesAsync(u);

        return new AdminUserDto
        {
            Id = u.Id,
            Email = u.Email ?? "",
            FirstName = u.FirstName ?? "",
            LastName = u.LastName ?? "",
            IsActive = u.IsActive,
            BusinessId = u.BusinessId,
            Roles = roles.ToList(),
            CreatedAt = u.CreatedAt
        };
    }

    public async Task UpdateIsActiveAsync(Guid userId, bool isActive, CancellationToken ct)
    {
        var u = await _userManager.FindByIdAsync(userId.ToString())
            ?? throw new NotFoundException("User", userId.ToString());

        u.IsActive = isActive;
        u.UpdatedAt = DateTimeOffset.UtcNow;

        var res = await _userManager.UpdateAsync(u);
        if (!res.Succeeded)
            throw new InvalidOperationException(string.Join("; ", res.Errors.Select(e => e.Description)));
    }

    public async Task DeleteAsync(Guid userId, CancellationToken ct)
    {
        var u = await _userManager.FindByIdAsync(userId.ToString())
            ?? throw new NotFoundException("User", userId.ToString());

        var res = await _userManager.DeleteAsync(u);
        if (!res.Succeeded)
            throw new InvalidOperationException(string.Join("; ", res.Errors.Select(e => e.Description)));
    }
}
