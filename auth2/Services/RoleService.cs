using auth2.Data;
using auth2.DTOs;
using auth2.DTOs.Roles;
using auth2.Middleware;
using auth2.Middleware.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace auth2.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<CreatedRoleDto> CreateRole(CreateRoleDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new AppException("Role name is required", 400);

            if (await _roleManager.RoleExistsAsync(dto.Name))
                throw new AppConflictException($"Role '{dto.Name}' already exists");

            IdentityRole role = new IdentityRole(dto.Name);

            IdentityResult? result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
                throw new AppException(
                    string.Join("; ", result.Errors.Select(e => e.Description)),
                    400
                );

            if (!result.Succeeded)
                throw new AppException(
                    string.Join("; ", result.Errors.Select(e => e.Description)),
                    400
                );

            return new CreatedRoleDto
            {
                Id = role.Id,
                Name = dto.Name
            };
        }

        public async Task<List<RoleDto>> GetRoles(string? userId = null)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return await _roleManager.Roles
                    .Select(r => new RoleDto
                    {
                        Id = r.Id,
                        Name = r.Name
                    })
                    .ToListAsync();
            }
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
