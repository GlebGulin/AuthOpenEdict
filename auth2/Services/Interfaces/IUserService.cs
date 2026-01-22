using auth2.Data;
using auth2.DTOs;
using auth2.DTOs.Users;

namespace auth2.Services
{
    public interface IUserService
    {
        Task<List<ApplicationUser>> GetUsers();
        
        Task<ApplicationUser> GetById(string id);
        Task<ApplicationUser> Delete(string id);
    }
}
