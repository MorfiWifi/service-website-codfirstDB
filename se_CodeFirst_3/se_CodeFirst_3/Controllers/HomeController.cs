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

        public async Task<ActionResult> EndUsersIndex()
        {
            var products = await helper.GetListOfItems<Product>("api/products");
            return View(products);
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

        [HttpGet]
        public ActionResult GetSettings()
        {
            UsefulMethodsHelper methodsHelper = new UsefulMethodsHelper();
            methodsHelper.LoadSettingFromWebConfig("SaveLogs");

            var a = System.Web.Configuration.WebConfigurationManager.AppSettings["SaveLogs"];

            List<SettingViewModel> settings = new List<SettingViewModel>();

            var appSetting = System.Web.Configuration.WebConfigurationManager.AppSettings;

            settings.Add(new SettingViewModel
            {
                SettingName = "SaveLogs",
                SettingValue = appSetting["SaveLogs"]
            });

            return View(settings);
        }

        public ActionResult PostSetting()
        {
            return View();
        }
        [HttpPost]
        [Route("Settings/SettingName/SettingValue")]
        public ActionResult PostSetting(string settingName, string settingValue)
        {
            var appSetting = System.Web.Configuration.WebConfigurationManager.AppSettings;
            appSetting.Set(settingName, settingValue);

            return View();
        }

        [HttpGet]
        [Route("Home/Statistics")]
        public async Task<ActionResult> Statistics()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ConnectToWebApiHelper helper = new ConnectToWebApiHelper();
            var usersCount = db.Users.Count();
            var suppliersCount = db.Suppliers.Count();
            var customtersCount = db.Customers.Count();
            var userSalaries = 0;

            foreach (var item in db.Users)
            {
                UserViewModel userInformation = await helper.GetItem<UserViewModel>("/api/users/" + item.Id);
                userSalaries += userInformation.FinalSalary;
            }

            List<SupplierViewModel2> suppliers = new List<SupplierViewModel2>();
            double allBuys = 0;
            foreach (var item in db.Suppliers)
            {
                //suppliers:
                SupplierInformationViewModel supplierInformation = await helper.GetItem<SupplierInformationViewModel>("api/suppliers/" + item.Id);
                suppliers.Add(new SupplierViewModel2
                {
                    ProductsCount = supplierInformation.Products.Count(),
                    MoneySpent = supplierInformation.Price,
                    SupplierCompanyName = supplierInformation.CompanyName
                });

                //buys:
                allBuys += supplierInformation.Price;
            }




            StatisticsViewModel statistics = new StatisticsViewModel
            {
                SuppliersCount = suppliersCount,
                CustomersCount = customtersCount,
                UserCounts = usersCount,
                UserSalaries = userSalaries,
                AllBuys = allBuys,
                Suppliers = suppliers,
            };


            return View(statistics);
        }
        //TODO
        //[HttpPost]
        //public ActionResult ContactUs()
        //{

        //}
    }
}
