using auth2.Data;
using auth2.DTOs;
using auth2.Services;
using auth2.Services.Interfaces;
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
        [HttpPost("attach-role")]
        public async Task<IActionResult> AttachRole([FromBody] RoleUserDto dto)
        {
            return Ok(await _userService.AttachRole(dto));
        }
        [HttpPost("detach-role")]
        public async Task<IActionResult> DetachRole([FromBody] RoleUserDto dto)
        {
            return Ok(await _userService.DetachRole(dto));
        }
    }
}
