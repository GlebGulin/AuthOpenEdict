using auth2.DTOs;
using auth2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace auth2.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserRolesController : ControllerBase
    {
        private readonly IUserRolesService _userRolesService;
        public UserRolesController(IUserRolesService userRolesService)
        {
            _userRolesService = userRolesService;        
        }
        [HttpPost("set")]
        public async Task<IActionResult> Set([FromBody] RoleUserDto dto)
        {
            return Ok(await _userRolesService.AttachRole(dto));
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] RoleUserDto dto)
        {
            return Ok(await _userRolesService.DetachRole(dto));
        }
        [Authorize]
        [HttpGet("{userId?}")]
        public async Task<IActionResult> GetUserRoles([FromRoute] string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("User ID must be provided.");
            return Ok(await _userRolesService.GetUserRoles(userId));
        }
    }
}
