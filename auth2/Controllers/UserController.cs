using auth2.Data;
using auth2.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace auth2.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users
                .Select(u => new { u.Id, u.UserName, u.Email }).ToListAsync();
            return Ok(users);
        }
        [HttpPost("set-role")]
        public async Task<IActionResult> SetRole([FromBody] SetRoleUserDto dto)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(dto.UserId);
            if (user is null)
                return NotFound($"User with id '{dto.UserId}' not found");

            IdentityRole? role = await _roleManager.FindByIdAsync(dto.RoleId);
            if (role is null)
                return NotFound($"Role with id '{dto.RoleId}' not found");

            if (await _userManager.IsInRoleAsync(user, role.Name!))
                return BadRequest("User already has this role");

            IdentityResult? result = await _userManager.AddToRoleAsync(user, role.Name!);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new
            {
                message = "Role assigned",
                userId = user.Id,
                role = role.Name
            });
        }
    }
}
