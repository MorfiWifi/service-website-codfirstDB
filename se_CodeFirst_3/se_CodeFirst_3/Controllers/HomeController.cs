using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using se_CodeFirst_3.Filters;
using se_CodeFirst_3.Helper;
using se_CodeFirst_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace se_CodeFirst_3.Controllers
{
    public class HomeController : Controller
    {
        ConnectToWebApiHelper helper = new ConnectToWebApiHelper();
        NotificationProviderHelper notificationHelper;

        public HomeController()
        {
            notificationHelper = new NotificationProviderHelper(this);
        }
        public ActionResult Test()
        {
            return View();
        }

#if DEBUG

#else
    [RedirectIfNotAuthorized]
#endif
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LogIn(LogInViewModel logInViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Dictionary<string, string> token = await helper.GetTokenDetails(logInViewModel.UserName, logInViewModel.Password);
                    HttpContext.Session["loginToken"] = token.Values.ElementAt(0);
                    
                    //5dd9e98e-3048-4e07-a6c5-c9b45460f136
                    if (HttpContext.Session["lastPageBeforeLogIn"] == null)
                    {
                        notificationHelper.CustomSuccessfulMessage("شما با موفقیت وارد شدید");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        notificationHelper.CustomSuccessfulMessage("شما با موفقیت وارد شدید");
                        var tempUrl = HttpContext.Session["lastPageBeforeLogIn"];
                        HttpContext.Session["lastPageBeforeLogIn"] = null;
                        return Redirect(tempUrl.ToString());
                    }
                }
                catch (Exception)
                {
                    notificationHelper.CustomFailureMessage("خطا در ورود:نام کاربری یا رمز عبور اشتباه است");
                }
            }

            return View(logInViewModel);
        }

        [HttpGet]
        public ActionResult LogOut()
        {
            bool successful = helper.LogOut("api/account/logout");

            if (successful)
            {
                notificationHelper.CustomSuccessfulMessage("شما با موفقیت خارج شدید");
                HttpContext.Session["loginToken"] = null;
            }
            else
            {
                notificationHelper.CustomFailureMessage("خطا در خروج");
            }
            return RedirectToAction("LogIn");
        }

        [HttpGet]
        public ActionResult AboutUs()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Blog()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ContactUS()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Help()
        {
            return View();
        }

        //TODO
        //[HttpPost]
        //public ActionResult ContactUs()
        //{

        //}
    }
}
