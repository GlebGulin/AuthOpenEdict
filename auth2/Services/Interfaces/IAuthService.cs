using auth2.DTOs;
using auth2.DTOs.Account;

namespace auth2.Services
{
    public interface IAuthService
    {
        Task<RegisterResponceDto> Register(RegisterRequestDto model);
    }
}
