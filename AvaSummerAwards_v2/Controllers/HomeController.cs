using System.Diagnostics;
using System.Threading.Tasks;
using Awards.Authentication;
using Awards.Helpers;
using Awards.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Awards.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            string userObjectID = (User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier"))?.Value;
            //var sessioncache = new NaiveSessionCache(userObjectID, HttpContext.Session);
            // Using ADAL.Net, get a bearer token to access the TodoListService
            AuthenticationContext authContext = new AuthenticationContext(AzureAdOptions.Settings.Authority, new NaiveSessionCache(userObjectID, HttpContext.Session));
            ClientCredential credential = new ClientCredential(AzureAdOptions.Settings.ClientId, AzureAdOptions.Settings.ClientSecret);
            var result = await authContext.AcquireTokenSilentAsync("https://graph.microsoft.com", credential, new UserIdentifier(userObjectID, UserIdentifierType.UniqueId));

            return View();
        }
        [HttpGet]
        public IActionResult SignOut()
        {
            // Remove all cache entries for this user and send an OpenID Connect sign-out request.
            string userObjectID = User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            var authContext = new AuthenticationContext(AzureAdOptions.Settings.Authority,
                                                        new NaiveSessionCache(userObjectID, HttpContext.Session));
            authContext.TokenCache.Clear();

            // Let Azure AD sign-out
            var callbackUrl = Url.Action(nameof(SignedOut), "Home", values: null, protocol: Request.Scheme);
            return SignOut(
                new AuthenticationProperties { RedirectUri = callbackUrl },
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public IActionResult SignedOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Redirect to home page if the user is authenticated.
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            return View();
        }
    }
}
