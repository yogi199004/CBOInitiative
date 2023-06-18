using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;




namespace AAPS.L10nPortal.Web.Controllers
{
    [Authorize]
    //[AllowAnonymous]
    public class HomeController : Controller
    {
        IAntiforgery _antiforgery;
        IConfiguration _config;




        public IActionResult Index()
        {
            //to implement anto forgery logic here
            //ViewBag.__RequestVerificationToken = TokenHeaderValue();
            //ViewBag.ApplicationInsights = _config.GetValue<string>("DeloitteCore:Logging:WriteTo:0:ConnectionString");
            //ViewBag.cookieDomain = _config.GetRequiredSection("CookieDomain").Value;
            return View();
        }

        public HomeController(IAntiforgery antiforgery, IConfiguration config)
        {
            _antiforgery = antiforgery;
            _config = config;
        }




        public string TokenHeaderValue()
        {
            var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
            var headerToken = tokens.RequestToken;
            return headerToken;
        }



        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var logoutUrl = _config.GetValue<string>("AzureAd:OpenIdLogOutURL");
            return Redirect(logoutUrl);
        }
    }
}