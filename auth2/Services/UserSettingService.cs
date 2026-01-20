using auth2.Data;
using auth2.Services.Interfaces;

namespace auth2.Services
{
    public class UserSettingService : IUserSettingService
    {
        private readonly AuthDbContext _context;
        public UserSettingService(AuthDbContext context)
        {
            _context = context;
        }
        public List<ApplicationUserSetting> Get(string userId)
        {
            return _context.ApplicationUserSetting.Where(us => us.UserId == userId).ToList();
        }
        public ApplicationUserSetting Upsert(string userId, string name, string value)
        {

            ApplicationUserSetting setting = _context.ApplicationUserSetting.FirstOrDefault(s => s.UserId == userId && s.Name == name);
            if (setting == null)
            {
                setting = new ApplicationUserSetting()
                {
                    UserId = userId,
                    Name = name,
                    Value = value
                };
                _context.ApplicationUserSetting.Add(setting);
            }
            else
            {
                setting.Value = value;
                _context.Update(setting);
            }
            _context.SaveChanges();

            return setting;
        }
    }
}
