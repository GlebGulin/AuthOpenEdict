using auth2.Data;
using auth2.Middleware;
using auth2.Services;
using auth2.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Validation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<AuthDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("users"));
    options.UseOpenIddict();
});
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<AuthDbContext>()
.AddDefaultTokenProviders();

// OpenIddict
builder.Services.AddOpenIddict()
    .AddCore(coreOptions =>
    {
        coreOptions.UseEntityFrameworkCore()
                   .UseDbContext<AuthDbContext>();
    })
    .AddServer(serverOptions =>
    {
        serverOptions.AllowPasswordFlow();
        serverOptions.AllowRefreshTokenFlow();
        serverOptions.AllowClientCredentialsFlow();
        serverOptions.SetTokenEndpointUris("/connect/token");
        serverOptions.RegisterScopes("api", "offline_access");

        serverOptions.AcceptAnonymousClients(); 

        serverOptions.AddDevelopmentEncryptionCertificate()
               .AddDevelopmentSigningCertificate();

        serverOptions.UseAspNetCore()
               .EnableTokenEndpointPassthrough();
    });
builder.Services.AddOpenIddict()
    .AddValidation(options =>
    {
        options.UseLocalServer();
        options.UseAspNetCore();
    });

// Auth & Authorization 
//builder.Services.AddAuthentication();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
        OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;

    options.DefaultChallengeScheme =
        OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
});
builder.Services.AddAuthorization();

// Worker
builder.Services.AddHostedService<Worker>();
// Seeders
builder.Services.AddHostedService<OpenIddictSeeder>();
builder.Services.AddHostedService<RolesSeeder>();

// Services
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IUserSettingService, UserSettingService>();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Middleware
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Run();
