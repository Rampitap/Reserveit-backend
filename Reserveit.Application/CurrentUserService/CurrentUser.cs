using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Reserveit.Application.CurrentUserService;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _http;

    public CurrentUser(IHttpContextAccessor http) => _http = http;

    public bool IsAuthenticated =>
        _http.HttpContext?.User?.Identity?.IsAuthenticated == true;

    public Guid UserId
    {
        get
        {
            var principal = _http.HttpContext?.User;
            if (principal?.Identity?.IsAuthenticated != true)
                throw new UnauthorizedAccessException("User is not authorized");

            var raw = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(raw) || !Guid.TryParse(raw, out var id))
                throw new UnauthorizedAccessException("Couldn't identify user");

            return id;
        }
    }
}
