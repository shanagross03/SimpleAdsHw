using SimpleAdsHw_3._13._24.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Security.Claims;
using Homework__3._13._24.Data;

namespace Homework__3._13._24.Web.Controllers
{
    public class AccountController : Controller
    {
        private AdsRepository _repo = new();

        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(User user, string password)
        {
            _repo.AddUser(user, password);
            return Redirect("/Account/Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string password, string email)
        {
            var user = _repo.Login(email, password);

            if (user == null)
            {
                TempData["Message"] = "Invalid Login!";
                return RedirectToAction("Login");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, email) 
            };

            HttpContext.SignInAsync(new ClaimsPrincipal(
                    new ClaimsIdentity(claims, "Cookies", ClaimTypes.Email, "roles"))
                ).Wait();

            return Redirect("/home/index");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect ("/Home/Index");
        }
    }
}
