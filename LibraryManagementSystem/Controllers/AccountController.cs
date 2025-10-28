using LibraryManagement.Models;
using LibraryManagement.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LibraryManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMemberRepository _members;
        private readonly IPasswordHasher<Member> _passwordHasher;

        public AccountController(IMemberRepository members, IPasswordHasher<Member> passwordHasher)
        {
            _members = members;
            _passwordHasher = passwordHasher;
        }

        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, string? returnUrl = null)
        {
            var member = _members.GetByEmail(email);

            if (member != null)
            {
                var result = _passwordHasher.VerifyHashedPassword(member, member.Password, password);

                if (result == PasswordVerificationResult.Success)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, member.Id.ToString()),
                        new Claim(ClaimTypes.Name, member.FullName),
                        new Claim(ClaimTypes.Email, member.Email),
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties();

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    return LocalRedirect(returnUrl ?? "/");
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}