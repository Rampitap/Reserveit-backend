using Reserveit.Application.Common.DTOs.UserDtos;

namespace Reserveit.Application.Interfaces;

public interface IProfileService
{
    Task ChangePasswordAsync(Guid userId, ChangePasswordRequestDto data, CancellationToken ct);
    Task DeleteAccountAsync(Guid userId, DeleteMyAccountRequestDto data, CancellationToken ct);
}
