using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace auth2.Services
{
    public class OpenIddictSeeder : IHostedService
    {
        private readonly IServiceProvider _sp;
        private readonly IConfiguration _config;

        public OpenIddictSeeder(IServiceProvider sp, IConfiguration config){
            _sp = sp;
            _config = config;
        }
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
                    ClientSecret = _config["Auth:Secret"],
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
            if (await manager.FindByClientIdAsync("mobile-client") == null){
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "mobile-client",
                    ClientSecret = _config["Auth:Secret"], Permissions =
                    {
                        Permissions.Endpoints.Token,
                        Permissions.GrantTypes.Password,
                        Permissions.GrantTypes.RefreshToken,
                        Permissions.Scopes.Profile,
                        Permissions.Prefixes.Scope + "api"
                    }
                });
            }
            if (await manager.FindByClientIdAsync("orders-service") == null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "orders-service",
                    ClientSecret = _config["Auth:Secret"],
                    Permissions =
                    {
                        Permissions.Endpoints.Token,
                        Permissions.GrantTypes.Password,
                        Permissions.GrantTypes.RefreshToken,
                        Permissions.Scopes.Profile,
                        Permissions.Prefixes.Scope + "api"
                    }
                });
            }
        }
        public Task StopAsync(CancellationToken ct) => Task.CompletedTask;
    }
}
