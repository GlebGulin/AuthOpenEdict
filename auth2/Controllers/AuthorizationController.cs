using auth2.Data;
using Microsoft.AspNetCore;
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
    public class AuthorizationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IOpenIddictApplicationManager _applicationManager;

        public AuthorizationController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager, IOpenIddictApplicationManager applicationManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _applicationManager = applicationManager;
        }
        //[HttpPost("~/connect/token")]
        //public async Task<IActionResult> Exchange()
        //{
        //    var request = HttpContext.GetOpenIddictServerRequest();

        //    if (!request.IsClientCredentialsGrantType())
        //        throw new InvalidOperationException();

        //    var application = await _applicationManager.FindByClientIdAsync(request.ClientId)
        //        ?? throw new InvalidOperationException();

        //    var identity = new ClaimsIdentity(
        //        TokenValidationParameters.DefaultAuthenticationType,
        //        Claims.Name,
        //        Claims.Role);

        //    identity.SetClaim(Claims.Subject, request.ClientId);
        //    identity.SetScopes("api");

        //    identity.SetDestinations(claim => new[]
        //    {
        //        Destinations.AccessToken
        //    });

        //    return SignIn(
        //        new ClaimsPrincipal(identity),
        //        OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        //}
        //[HttpPost("~/connect/token")]
        //public async Task<IActionResult> Exchange()
        //{
        //    var request = HttpContext.GetOpenIddictServerRequest();

        //    if (request.IsPasswordGrantType())
        //    {
        //        var user = await _userManager.FindByNameAsync(request.Username);

        //        if (user == null)
        //            return Forbid();

        //        var result = await _signInManager.CheckPasswordSignInAsync(
        //            user, request.Password, false);

        //        if (!result.Succeeded)
        //            return Forbid();

        //        var identity = new ClaimsIdentity(
        //            OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        //        identity.SetClaim(Claims.Subject, user.FullName);
        //        identity.SetClaim(Claims.Email, user.Email);
        //        identity.SetScopes(request.GetScopes());
        //        identity.SetResources("api");

        //        identity.SetDestinations(c => new[] { Destinations.AccessToken });

        //        return SignIn(
        //            new ClaimsPrincipal(identity),
        //            OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        //    }

        //    throw new InvalidOperationException();
        //}
        [HttpPost("~/connect/token")]
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

            throw new InvalidOperationException("Unsupported grant type.");
        }

    }
}
