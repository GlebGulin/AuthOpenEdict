using auth2.Data;

namespace auth2.Services.Interfaces
{
    public interface IUserSettingService
    {
        List<ApplicationUserSetting> Get(string userId);
        ApplicationUserSetting Upsert(string userId, string name, string value, string type);
        void Delete(int Id);
    }
}
