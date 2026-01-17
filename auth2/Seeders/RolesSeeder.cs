using Microsoft.AspNetCore.Identity;

namespace auth2.Services
{
    public class RolesSeeder : IHostedService
    {
        private readonly IServiceProvider _sp;

        public RolesSeeder(IServiceProvider sp) => _sp = sp;

        public async Task StartAsync(CancellationToken ct)
        {
            using var scope = _sp.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles = { "Admin", "User", "Service", "Manager", "Creator", "Animator" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        public Task StopAsync(CancellationToken ct) => Task.CompletedTask;
    }
}
