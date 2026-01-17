using auth2.DTOs;
using auth2.DTOs.Roles;

namespace auth2.Services
{
    public interface IRoleService
    {
        Task<List<RoleDto>> GetRoles(string userId = null);
        Task<CreatedRoleDto> CreateRole(CreateRoleDto dto);
    }
}
