using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TalentManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        [HttpGet("/account/login")]
        public IActionResult Login() => View();

        [HttpPost("/account/login")]
        public async Task<IActionResult> Login([FromForm] string userId, [FromForm] string password)
        {
            if(userId == password)
            {
                var claims = new List<Claim>
                {
                    new Claim (ClaimTypes.Name, userId),
                    new Claim(ClaimTypes.Role, "Human Resources Manager"),
                    new Claim("Department", "IT"),
                    new Claim("Country", "USA"),
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid Creds";
            return View();
        }

        [HttpPost("/account/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
