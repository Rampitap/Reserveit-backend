namespace Reserveit.Application.Interfaces;

public interface IUserAccountService
{
    Task SetIsActiveAsync(Guid userId, bool isActive, CancellationToken ct);
    Task DeleteUserAsync(Guid userId, CancellationToken ct);
}
