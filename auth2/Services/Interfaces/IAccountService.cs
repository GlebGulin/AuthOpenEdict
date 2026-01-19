using auth2.DTOs;
using auth2.DTOs.Account;

namespace auth2.Services.Interfaces
{
    public interface IAccountService
    {
        Task<RegisterResponceDto> Register(RegisterRequestDto model);
    }
}
