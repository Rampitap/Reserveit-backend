using Reserveit.Application.Common.DTOs.AdminManageDtos;
using Reserveit.Application.Common.Pagination;

namespace Reserveit.Application.Interfaces;

public interface IAdminUserRepository
{
    Task<PagedResult<AdminUserDto>> GetPagedAsync(int page, int pageSize, string? q, CancellationToken ct);
    Task<AdminUserDto> GetByIdAsync(Guid userId, CancellationToken ct);

    Task UpdateIsActiveAsync(Guid userId, bool isActive, CancellationToken ct);
    Task DeleteAsync(Guid userId, CancellationToken ct);
}
