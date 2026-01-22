using auth2.DTOs;
using auth2.DTOs.Users;

namespace auth2.Services
{
    public interface IUserRolesService
    {
        Task<ResultRoleUserDto> AttachRole(RoleUserDto dto);
        Task<ResultRoleUserDto> DetachRole(RoleUserDto dto);
        Task<List<RoleDto>> GetUserRoles(string userId);
    }
}
