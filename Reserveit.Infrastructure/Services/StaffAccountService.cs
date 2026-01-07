using Microsoft.AspNetCore.Identity;
using Reserveit.Application.Common.DTOs.StaffDtos;
using Reserveit.Application.Interfaces;
using Reserveit.Domain.Constants;
using Reserveit.Domain.Entities;
using Reserveit.Infrastructure.Persistence;

namespace Reserveit.Infrastructure.Services;

public sealed class StaffAccountService : IStaffAccountService
{
    private readonly AppDbContext _db;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public StaffAccountService(
        AppDbContext db,
        UserManager<User> userManager,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        _db = db;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<Guid> CreateStaffAsync(CreateStaffAccountDto dto, CancellationToken ct)
    {
        // 1) email unique
        var existing = await _userManager.FindByEmailAsync(dto.Email);
        if (existing != null)
            throw new ArgumentException("User with this email already exists.");

        // 2) ensure role exists
        if (!await _roleManager.RoleExistsAsync(UserRoles.Staff))
            throw new InvalidOperationException($"Role '{UserRoles.Staff}' does not exist.");

        await using var tx = await _db.Database.BeginTransactionAsync(ct);

        // 3) create identity user
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = dto.Email,
            Email = dto.Email,

            FirstName = dto.FirstName ?? string.Empty,
            LastName = dto.LastName ?? string.Empty,

            BusinessId = dto.BusinessId,
            IsActive = true,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        var createResult = await _userManager.CreateAsync(user, dto.Password);
        if (!createResult.Succeeded)
        {
            var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
            throw new ArgumentException($"User creation failed: {errors}");
        }

        var roleResult = await _userManager.AddToRoleAsync(user, UserRoles.Staff);
        if (!roleResult.Succeeded)
        {
            var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
            throw new ArgumentException($"Assigning role failed: {errors}");
        }

        // 4) create Staff entity
        var staff = new Staff
        {
            Id = Guid.NewGuid(),
            BusinessId = dto.BusinessId,
            UserId = user.Id,
            DisplayName = dto.DisplayName,
            Bio = dto.Bio,
            IsActive = true,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        _db.Staffs.Add(staff);
        await _db.SaveChangesAsync(ct);

        await tx.CommitAsync(ct);

        return staff.Id;
    }
}
