using auth2.Data;
using auth2.DTOs;
using auth2.DTOs.Roles;
using auth2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace auth2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [Authorize]
        [HttpGet("{userId?}")]
        public async Task<ActionResult<List<RoleDto>>> GetRoles(string userId = null)
        {
            return Ok(await _roleService.GetRoles(userId));
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<CreatedRoleDto>> CreateRole([FromBody] CreateRoleDto dto)
        {
            return Ok(await _roleService.CreateRole(dto));
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult>DeleteRole([FromRoute] string id)
        {
            return Ok(await _roleService.DeleteRole(id));
        }
    }
}
