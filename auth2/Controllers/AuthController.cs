using auth2.Data;
using auth2.DTOs;
using auth2.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace auth2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAuthService _authService;

        public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager, IAuthService authService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto model)
        {
            return Ok(await _authService.Register(model));
        }
        [HttpPost("login")]
        public async Task<IActionResult> Exchange()
        {
            var request = HttpContext.GetOpenIddictServerRequest()
                ?? throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            if (request.IsPasswordGrantType())
            {
                var user = await _userManager.FindByNameAsync(request.Username);
                if (user is null)
                    return Forbid(
                        OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

                var result = await _signInManager.CheckPasswordSignInAsync(
                    user, request.Password, lockoutOnFailure: false);

                if (!result.Succeeded)
                    return Forbid(
                        OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

                var identity = new ClaimsIdentity(
                    OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

               
                identity.SetClaim(Claims.Subject, user.Id);

                identity.SetClaim(Claims.Email, user.Email);
                identity.SetClaim(Claims.Name, user.UserName);
                var roles = await _userManager.GetRolesAsync(user);
                foreach (var role in roles)
                {
                    identity.AddClaim(new Claim(Claims.Role, role));
                }
                identity.SetScopes(request.GetScopes());
                identity.SetResources("api");

                identity.SetDestinations(claim => new[]
                {
                    Destinations.AccessToken,
                    Destinations.IdentityToken
                });

                return SignIn(
                    new ClaimsPrincipal(identity),
                    OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
            if (request.IsRefreshTokenGrantType())
            {
                var result = await HttpContext.AuthenticateAsync(
                    OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

                var identity = new ClaimsIdentity(result.Principal.Claims,
                    OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

                identity.SetScopes(request.GetScopes());
                identity.SetResources("api");

                identity.SetDestinations(claim => new[]
                {
                    Destinations.AccessToken
                });

                return SignIn(
                    new ClaimsPrincipal(identity),
                    OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
            throw new InvalidOperationException("Unsupported grant type.");
        }

    }
}
