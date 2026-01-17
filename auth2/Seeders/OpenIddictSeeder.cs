using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace auth2.Services
{
    public class OpenIddictSeeder : IHostedService
    {
        private readonly IServiceProvider _sp;

        public OpenIddictSeeder(IServiceProvider sp)
            => _sp = sp;

        public async Task StartAsync(CancellationToken ct)
        {
            using var scope = _sp.CreateScope();

            var manager = scope.ServiceProvider
                .GetRequiredService<IOpenIddictApplicationManager>();

            if (await manager.FindByClientIdAsync("web-client") == null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "web-client",
                    ClientSecret = "secret",
                    Permissions =
                {
                    //Permissions.Endpoints.Token,
                    //Permissions.GrantTypes.Password,
                    //Permissions.GrantTypes.RefreshToken,
                    //Permissions.Scopes.Profile,
                    //Permissions.Prefixes.Scope + "api"
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.Password,
                    Permissions.GrantTypes.RefreshToken,
                    Permissions.Scopes.Profile,
                    //Permissions.Scopes.OfflineAccess,
                    Permissions.Prefixes.Scope + "api"
                }
                });
            }
        }
        public Task StopAsync(CancellationToken ct) => Task.CompletedTask;
    }
}
