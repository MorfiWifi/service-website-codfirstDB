using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using se_CodeFirst_3.Helper;
using se_CodeFirst_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace se_CodeFirst_3.Controllers
{
    public class HomeController : Controller
    {
        ConnectToWebApiHelper helper = new ConnectToWebApiHelper();

        public ActionResult Test()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var supplier = db.Suppliers.Find(1);

            var a = DateTimeOffset.Now;
            var a1 = DateTimeOffset.UtcNow;
            var a2 = DateTimeOffset.MinValue;
            var a3 = DateTimeOffset.MaxValue;

            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();

            DateTime datetime = new DateTime();
            datetime = DateTime.Now;

            var c = pc.GetDayOfWeek(datetime);
            var c2 = pc.GetDayOfMonth(datetime);
            var c3 = pc.GetDayOfYear(datetime);
            var c4 = pc.GetHour(datetime);
            var c5 = pc.GetMinute(datetime);
            var c6 = pc.GetMonth(datetime);
            var c7 = pc.GetSecond(datetime);

            //var user = db.Users.Find(1);

            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);

            var bb = userManager.GetRoles((from item in db.Users
                                  where item.UserName == "admin"
                                  select item.Id).SingleOrDefault());

            return View(supplier.Products);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AdminPanel()
        {
            ViewBag.Title = "Admin Panel";

            return View();
        }

        public ActionResult AdminPanelCustomer()
        {
            return View();
        }

        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(LogInViewModel logInViewModel)
        {
            if (ModelState.IsValid)
            {
                //helper.GetTokenDetails(logInViewModel.UserName, logInViewModel.Password);
                //string val0 = logInViewModel.UserName;
                Dictionary<string, string> token = new Dictionary<string, string>();
                token = helper.GetTokenDetails("admin", "bbBB11!!");
                HttpContext.Session["loginToken"] = token.Values.ElementAt(0);
                    //HttpContext.Session["loginToken"] = token.Values.Last();
                    ViewBag.Title = Session["loginToken"];
                    return RedirectToAction("Index");
                
                
                
            }
            else
            {
                ViewBag.Title = "خطا در ورود کاربر";
            }
            return View(logInViewModel);
        }

        [HttpGet]
        public ActionResult LogOut()
        {
            bool successful = helper.LogOut("api/account/logout");

            if (successful)
            {
                HttpContext.Session["loginToken"] = null;
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
