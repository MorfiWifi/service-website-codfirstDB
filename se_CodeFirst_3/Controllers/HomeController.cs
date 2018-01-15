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
using PagedList;

namespace se_CodeFirst_3.Controllers
{
    public class HomeController : Controller
    {
        ConnectToWebApiHelper helper = new ConnectToWebApiHelper();
        NotificationProviderHelper notificationHelper;
        UsefulMethodsHelper methodHelper = new UsefulMethodsHelper();

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

        public async Task<ActionResult> EndUsersIndex(int? page, bool? searching, ProductDTO productDTO)
        {
            bool castedSearching = searching.HasValue ? searching.Value : false;

            List<Product> products = new List<Product>();

            if (productDTO != null)
            {
                products = await helper.GetListOfItems<Product>("api/productsNoAuthorization",
                    "?Name=" + productDTO.Name + "&" +
                    "SupplierCompanyName=" + productDTO.SupplierCompanyName + "&" +
                    "MinUnitsInStock=" + productDTO.MinUnitsInStock + "&" +
                    "MaxUnitsInStock=" + productDTO.MaxUnitsInStock + "&" +
                    "MinUnitPrice=" + productDTO.MinUnitPrice + "&" +
                    "MaxUnitPrice=" + productDTO.MaxUnitPrice + "&" +
                    "MinSellUnitPrice=" + productDTO.MinSellUnitPrice + "&" +
                    "MaxSellUnitPrice=" + productDTO.MaxSellUnitPrice
                    );
            }
            else
            {
                products = await helper.GetListOfItems<Product>("api/productsNoAuthorization");
            }

            if (castedSearching)
            {
                int pageSize = 18;
                int pageNumber = (page ?? 1);
                return Json(products.ToPagedList(pageNumber, pageSize), JsonRequestBehavior.AllowGet);
            }
            else
            {
                int pageSize = 18;
                int pageNumber = (page ?? 1);
                return View(products.ToPagedList(pageNumber, pageSize));
            }
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
        public async Task<ActionResult> Statistics(bool? searching, IncomingCallDTO incomingCallDTO)
        {
            bool castedSearching = searching.HasValue ? searching.Value : false;


            ApplicationDbContext db = new ApplicationDbContext();
            ConnectToWebApiHelper helper = new ConnectToWebApiHelper();
            var usersCount = db.Users.Count();
            var suppliersCount = db.Suppliers.Count();
            var customtersCount = db.Customers.Count();
            var userSalaries = 0;
            double allBuys = 0;
            double allSells = 0;
            double allProfits = 0;

            if (castedSearching == false)
            {
                foreach (var item in db.Users)
                {
                    UserViewModel userInformation = await helper.GetItem<UserViewModel>("/api/users/" + item.Id);
                    userSalaries += userInformation.FinalSalary;
                }

                List<SupplierViewModel2> suppliers = new List<SupplierViewModel2>();

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

                List<CustomerViewModel2> customers = new List<CustomerViewModel2>();
                foreach (var item in db.Customers)
                {
                    int customerProductsCount = (from item2 in item.Orders
                                                 join item3 in db.Order_Details on item2.Id equals item3.OrderId
                                                 select item3.Quantity).Sum();

                    int customerBuysCount = (from item2 in item.Orders
                                             join item3 in db.Order_Details on item2.Id equals item3.OrderId
                                             select item3.Product.BuyUnitPrice * item3.Quantity).Sum();

                    int customerSellsCount = (from item2 in item.Orders
                                              join item3 in db.Order_Details on item2.Id equals item3.OrderId
                                              select item3.Product.SellUnitPrice * item3.Quantity).Sum();

                    int customerProfitsGained = customerSellsCount - customerBuysCount;

                    customers.Add(new CustomerViewModel2
                    {
                        Id = item.Id,
                        CustomerName = item.Name,
                        ProductsCount = customerProductsCount,
                        ProfitsGained = customerProfitsGained
                    });

                    allSells += customerSellsCount;
                }

                allProfits = allSells - allBuys;


                //Get Statistics fot last month::


                DateTime oneMonthBefore = DateTime.Now;
                oneMonthBefore = oneMonthBefore.AddMonths(-1);
                OrderDTO orderDTO = new OrderDTO()
                {
                    Content = "",
                    Customer = "",
                    MinRequiredDate = oneMonthBefore,
                    MaxRequiredDate = DateTime.Now,
                    MinOrderDate = oneMonthBefore.AddYears(-5),
                    MaxOrderDate = oneMonthBefore.AddYears(5)
                };

                List<Order> orders = await helper.GetListOfItems<Order>("api/orders",
                        "?Content=" + orderDTO.Content + "&" +
                        "Customer=" + orderDTO.Customer + "&" +
                        "MinOrderDate=" + orderDTO.MinOrderDate + "&" +
                        "MaxOrderDate=" + orderDTO.MaxOrderDate + "&" +
                        "MinRequiredDate=" + orderDTO.MinRequiredDate + "&" +
                        "MaxRequiredDate=" + orderDTO.MaxRequiredDate
                        );


                var lastMonthSells = (from item in orders
                                      join item2 in db.Order_Details on item.Id equals item2.OrderId
                                      select item2.Quantity * item2.Product.SellUnitPrice).Sum();

                var temp = (from item in orders
                            join item2 in db.Order_Details on item.Id equals item2.OrderId
                            select item2.Quantity * item2.Product.BuyUnitPrice).Sum();

                var lastMonthBenefits = lastMonthSells - temp;

                //Get Statistics for last Year::
                orderDTO = new OrderDTO()
                {
                    Content = "",
                    Customer = "",
                    MinRequiredDate = oneMonthBefore.AddMonths(1).AddYears(-1),
                    MaxRequiredDate = DateTime.Now,
                    MinOrderDate = oneMonthBefore.AddYears(-5),
                    MaxOrderDate = oneMonthBefore.AddYears(5)
                };

                orders = await helper.GetListOfItems<Order>("api/orders",
                        "?Content=" + orderDTO.Content + "&" +
                        "Customer=" + orderDTO.Customer + "&" +
                        "MinOrderDate=" + orderDTO.MinOrderDate + "&" +
                        "MaxOrderDate=" + orderDTO.MaxOrderDate + "&" +
                        "MinRequiredDate=" + orderDTO.MinRequiredDate + "&" +
                        "MaxRequiredDate=" + orderDTO.MaxRequiredDate
                        );

                var lastYearSells = (from item in orders
                                     join item2 in db.Order_Details on item.Id equals item2.OrderId
                                     select item2.Quantity * item2.Product.SellUnitPrice).Sum();

                temp = (from item in orders
                        join item2 in db.Order_Details on item.Id equals item2.OrderId
                        select item2.Quantity * item2.Product.BuyUnitPrice).Sum();

                var lastYearBenefits = lastYearSells - temp;

                StatisticsViewModel statistics = new StatisticsViewModel
                {
                    SuppliersCount = suppliersCount,
                    CustomersCount = customtersCount,
                    UserCounts = usersCount,
                    UserSalaries = userSalaries,
                    AllBuys = allBuys,
                    AllSells = allSells,
                    AllProfits = allProfits,
                    Suppliers = suppliers,
                    Customers = customers,
                    LastMonthBuys = 0,
                    LastMonthSells = lastMonthSells,
                    LastMonthProfitss = lastMonthBenefits,
                    LastYearBuys = 0,
                    LastYearSells = lastYearSells,
                    LastYearProfits = lastYearBenefits
                };

                return View(statistics);
            }
            else
            {
                //Get Statistics for Custom Dates::

                OrderDTO orderDTO = new OrderDTO()
                {
                    Content = "",
                    Customer = "",
                    MinRequiredDate = methodHelper.ConvertDateTimeToGregorian(incomingCallDTO.MinDate),
                    MaxRequiredDate = DateTime.Now,
                    MinOrderDate = DateTime.Now.AddYears(-50),
                    MaxOrderDate = DateTime.Now.AddYears(50)
                };

                List<Order> orders = await helper.GetListOfItems<Order>("api/orders",
                        "?Content=" + orderDTO.Content + "&" +
                        "Customer=" + orderDTO.Customer + "&" +
                        "MinOrderDate=" + orderDTO.MinOrderDate + "&" +
                        "MaxOrderDate=" + orderDTO.MaxOrderDate + "&" +
                        "MinRequiredDate=" + orderDTO.MinRequiredDate + "&" +
                        "MaxRequiredDate=" + orderDTO.MaxRequiredDate
                        );


                var customDateSells = (from item in orders
                                       join item2 in db.Order_Details on item.Id equals item2.OrderId
                                       select item2.Quantity * item2.Product.SellUnitPrice).Sum();

                var temp = (from item in orders
                            join item2 in db.Order_Details on item.Id equals item2.OrderId
                            select item2.Quantity * item2.Product.BuyUnitPrice).Sum();

                var customDateBenefits = customDateSells - temp;

                StatisticsViewModel statistics = new StatisticsViewModel
                {
                    CustomDateBuys = 0,
                    CustomDateSells = customDateSells,
                    CustomDateProfits = customDateBenefits
                };
                return Json(statistics, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
