using auth2.DTOs;
using auth2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace auth2.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _userService.GetUsers());
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> Get([FromRoute] string userId)
        {
            return Ok(await _userService.GetById(userId));
        }

        [HttpPost("attach-role")]
        public async Task<IActionResult> AttachRole([FromBody] RoleUserDto dto)
        {
            return Ok(await _userService.AttachRole(dto));
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("detach-role")]
        public async Task<IActionResult> DetachRole([FromBody] RoleUserDto dto)
        {
            return Ok(await _userService.DetachRole(dto));
        }
    }
}
