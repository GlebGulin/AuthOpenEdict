using auth2.Data;
using auth2.DTOs;
using auth2.DTOs.Users;

namespace auth2.Services
{
    public interface IUserService
    {
        Task<List<ApplicationUser>> GetUsers();
        Task<ResultRoleUserDto> AttachRole(RoleUserDto dto);
        Task<ResultRoleUserDto> DetachRole(RoleUserDto dto);
        Task<ApplicationUser> GetById(string id);
    }
}
