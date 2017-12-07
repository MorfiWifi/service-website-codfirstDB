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
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            //var a = db.Users.Find(1);
            ViewBag.Title = db.Users.ToList();
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
    }
}
