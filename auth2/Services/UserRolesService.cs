using auth2.Data;
using auth2.DTOs;
using auth2.DTOs.Users;
using auth2.Middleware;
using auth2.Middleware.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace auth2.Services
{
    public class UserRolesService : IUserRolesService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserRolesService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
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
                throw new AppException(string.Join(",", result.Errors.Select(x => x.Code).ToList()), 400);

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

        public async Task<List<RoleDto>> GetUserRoles(string userId)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                throw new Exception($"User with id '{userId}' not found");

            IList<string>? userRoles = await _userManager.GetRolesAsync(user);

            return await _roleManager.Roles
                .Where(r => userRoles.Contains(r.Name!))
                .Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name
                })
                .ToListAsync();
        }
    }
}
