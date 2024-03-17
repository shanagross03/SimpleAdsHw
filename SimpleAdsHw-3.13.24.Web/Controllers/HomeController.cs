using SimpleAdsHw_3._13._24.Web;
using SimpleAdsHw_3._13._24.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Homework__3._13._24.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Homework__3._13._24.Web.Controllers
{
    public class HomeController : Controller
    {
        private AdsRepository _repo = new();

        public IActionResult Index()
        {
            return View(new AdViewModel()
            {
                Ads = _repo.GetAds(),
                UserId = User.Identity.IsAuthenticated ? _repo.GetUserByEmail(User.Identity.Name).Id : 0
            });
        }

        public IActionResult DeleteAd(int id)
        {
            _repo.DeleteAd(id);
            return Redirect("/home/index");
        }

        public IActionResult MyAccount()
        {
            return View(new AdViewModel()
            {
                Ads = _repo.GetAds(_repo.GetUserByEmail(User.Identity.Name).Id)
            });
        }

        public IActionResult NewAd()
        {
            return !User.Identity.IsAuthenticated ? Redirect("/Account/Login") : View();
        }

        [HttpPost]
        public IActionResult NewAd(Ad ad)
        {
            ad.UserId = _repo.GetUserByEmail(User.Identity.Name).Id;
            _repo.NewAd(ad);
            return Redirect("/Home/Index");
        }
    }
}