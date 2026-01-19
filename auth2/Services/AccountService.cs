using auth2.Data;
using auth2.DTOs;
using auth2.DTOs.Account;
using auth2.Middleware;
using auth2.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace auth2.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<RegisterResponceDto> Register(RegisterRequestDto model)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                throw new AppException(string.Join(",", result.Errors.Select(x => x.Description)), 400);
            await _userManager.AddToRoleAsync(user, "User");
            return new RegisterResponceDto() { Id = user.Id };
        }
    }
}
