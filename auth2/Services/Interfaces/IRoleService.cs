using auth2.DTOs;
using auth2.DTOs.Roles;
using Microsoft.AspNetCore.Mvc;

namespace auth2.Services
{
    public interface IRoleService
    {
        Task<List<RoleDto>> GetRoles(string userId = null);
        Task<CreatedRoleDto> CreateRole(CreateRoleDto dto);
        Task<ResultDeleteRoleDto> DeleteRole(string id);
    }
}
