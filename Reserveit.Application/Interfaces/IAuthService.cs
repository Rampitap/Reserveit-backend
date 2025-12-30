using Reserveit.Application.Common.DTOs.AuthDtod;
using System.Security.Claims;

namespace Reserveit.Application.Interfaces;

public interface IAuthService
{
    
        
        public Task<ResponseDto> RegisterAsync(RegisterDto dto);

        
        public Task<ResponseDto> LoginAsync(LoginDto dto);

        
        public Task LogoutAsync();

        
        public Task<ResponseDto> GetCurrentUserAsync(ClaimsPrincipal principal);
    
}
