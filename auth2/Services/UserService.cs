using auth2.Data;
using auth2.DTOs;
using auth2.DTOs.Users;
using auth2.Middleware;
using auth2.Middleware.Exceptions;
using auth2.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace auth2.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<List<ApplicationUser>> GetUsers()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<ResultRoleUserDto> AttachRole(RoleUserDto dto)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(dto.UserId);
            if (user is null)
                throw new AppNotFoundException($"User with id '{dto.UserId}' not found");

            IdentityRole? role = await _roleManager.FindByIdAsync(dto.RoleId);
            if (role is null)
                throw new AppNotFoundException($"Role with id '{dto.RoleId}' not found");

            if (await _userManager.IsInRoleAsync(user, role.Name!))
                throw new AppException("User already has this role", 400);

            IdentityResult? result = await _userManager.AddToRoleAsync(user, role.Name!);

            if (!result.Succeeded)
                throw new AppException(string.Join(",", result.Errors.Select(x => x.Code).ToList()) , 400);

            return new ResultRoleUserDto
            {
                Message = "Role assigned",
                UserId = user.Id,
                Role = role.Name
            };
        }

        public async Task<ResultRoleUserDto> DetachRole(RoleUserDto dto)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(dto.UserId);
            if (user is null)
                throw new AppNotFoundException($"User with id '{dto.UserId}' not found");

            IdentityRole? role = await _roleManager.FindByIdAsync(dto.RoleId);
            if (role is null)
                throw new AppNotFoundException($"Role with id '{dto.RoleId}' not found");

            if (!await _userManager.IsInRoleAsync(user, role.Name!))
                throw new AppException("User does not have this role", 400);

            IdentityResult result = await _userManager.RemoveFromRoleAsync(user, role.Name!);

            if (!result.Succeeded)
                throw new AppException(
                    string.Join(",", result.Errors.Select(x => x.Code)),
                    400
                );

            return new ResultRoleUserDto
            {
                Message = "Role detached",
                UserId = user.Id,
                Role = role.Name
            };
        }

        public async Task<ApplicationUser> GetById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }
    }
}
