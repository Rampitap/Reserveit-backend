namespace Reserveit.Application.CurrentUserService;

public interface ICurrentUser
{
    Guid UserId { get; }
    bool IsAuthenticated { get; }
}
