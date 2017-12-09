using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using se_CodeFirst_3.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace se_CodeFirst_3.Helper
{
    public class UsefulMethodsHelper
    {
        public void InitializationOfUsersPasswordAndRoles(ApplicationDbContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);


            //this is our first 4 roles in database:
            roleManager.Create(new IdentityRole
            {
                Name = "Administrator"
            });

            roleManager.Create(new IdentityRole
            {
                Name = "Accountant"
            });

            roleManager.Create(new IdentityRole
            {
                Name = "Secretary"
            });

            roleManager.Create(new IdentityRole
            {
                Name = "StoreKeeper"
            });


            var admin = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@gmail.com"
            };

            var accountant = new ApplicationUser
            {
                UserName = "accountant",
                Email = "accountant@gmail.com"
            };

            var secretary = new ApplicationUser
            {
                UserName = "secretary",
                Email = "secretary@gmail.com"
            };

            var storeKeeper = new ApplicationUser
            {
                UserName = "storekeeper",
                Email = "storekeeper@gmail.com"
            };

            //admin.Salary = 5000;
            //secretary.Salary = 4000;
            //accountant.Salary = 3000;
            //storeKeeper.Salary = 2000;

            userManager.Create(admin);
            userManager.Create(accountant);
            userManager.Create(secretary);
            userManager.Create(storeKeeper);

            userManager.AddToRole(admin.Id, "Administrator");
            userManager.AddToRole(accountant.Id, "Accountant");
            userManager.AddToRole(secretary.Id, "Secretary");
            userManager.AddToRole(storeKeeper.Id, "StoreKeeper");

            userManager.AddPassword(admin.Id, "bbBB11!!");
            userManager.AddPassword(accountant.Id, "bbBB11!!");
            userManager.AddPassword(secretary.Id, "bbBB11!!");
            userManager.AddPassword(storeKeeper.Id, "bbBB11!!");

            



            //userManager.AddClaimAsync(admin.Id, claim);


        }

        public void FillClaimsTable(ApplicationDbContext context)
        {
        //    List<ClaimViewModel> claims = new List<ClaimViewModel>
        //    {
        //        new ClaimViewModel("IncomingCall", "Get", "تماس های ورودی", "مشاهده"),
        //        new ClaimViewModel("IncomingCall", "Post", "تماس های ورودی", "افزودن"),
        //        new ClaimViewModel("IncomingCall", "Put", "تماس های ورودی", "تغییر"),
        //        new ClaimViewModel("IncomingCall", "Delete", "تماس های ورودی", "حذف"),

        //        new ClaimViewModel("Supplier", "Get", "تولیدکنندگان", "مشاهده"),
        //        new ClaimViewModel("Supplier", "Post", "تولیدکنندگان", "افزودن"),
        //        new ClaimViewModel("Supplier", "Put", "تولیدکنندگان", "تغییر"),
        //        new ClaimViewModel("Supplier", "Delete", "تولیدکنندگان", "حذف"),

        //        new ClaimViewModel("Product", "Get", "کالا", "مشاهده"),
        //        new ClaimViewModel("Product", "Post", "کالا", "افزودن"),
        //        new ClaimViewModel("Product", "Put", "کالا", "تغییر"),
        //        new ClaimViewModel("Product", "Delete", "کالا", "حذف"),

        //        new ClaimViewModel("Order_Detail", "Get", "جزئیات قرارداد", "مشاهده"),
        //        new ClaimViewModel("Order_Detail", "Post", "جزئیات قرارداد", "افزودن"),
        //        new ClaimViewModel("Order_Detail", "Put", "جزئیات قرارداد", "تغییر"),
        //        new ClaimViewModel("Order_Detail", "Delete", "جزئیات قرارداد", "حذف"),

        //        new ClaimViewModel("Order", "Get", "سفارش", "مشاهده"),
        //        new ClaimViewModel("Order", "Post", "سفارش", "افزودن"),
        //        new ClaimViewModel("Order", "Put", "سفارش", "تغییر"),
        //        new ClaimViewModel("Order", "Delete", "سفارش", "حذف"),

        //        new ClaimViewModel("Customer", "Get", "مشتری", "مشاهده"),
        //        new ClaimViewModel("Customer", "Post", "مشتری", "افزودن"),
        //        new ClaimViewModel("Customer", "Put", "مشتری", "تغییر"),
        //        new ClaimViewModel("Customer", "Delete", "مشتری", "حذف"),

        //        new ClaimViewModel("Contract", "Get", "متن قرارداد", "مشاهده"),
        //        new ClaimViewModel("Contract", "Post", "متن قرارداد", "افزودن"),
        //        new ClaimViewModel("Contract", "Put", "متن قرارداد", "تغییر"),
        //        new ClaimViewModel("Contract", "Delete", "متن قرارداد", "حذف")

        //        //new ClaimViewModel("User", "Get"),
        //        //new ClaimViewModel("User", "Post"),
        //        //new ClaimViewModel("User", "Put"),
        //        //new ClaimViewModel("User", "Delete")


        //};
        //    claims.ForEach(item => context.Claims.Add(item));
        //    context.SaveChanges();


        }

        public DateTime ConvertDateTimeToGregorian(DateTime dateTime)
        {
            PersianCalendar pc = new PersianCalendar();

            var convertedDateTime = pc.ToDateTime(dateTime.Year, dateTime.Month, dateTime.Day,
                dateTime.Hour, dateTime.Minute + 1, dateTime.Second, dateTime.Millisecond);

            return convertedDateTime;
        }

        public DateTime ConvertDateTimeToPersian(DateTime gregorianDateTime)
        {
            DateTime dateTime = DateTime.Parse(gregorianDateTime.ToString());
            PersianCalendar pc = new PersianCalendar();

            DateTime result = new DateTime(pc.GetYear(dateTime), pc.GetMonth(dateTime), pc.GetDayOfMonth(dateTime),
                pc.GetHour(dateTime), pc.GetMinute(dateTime), pc.GetSecond(dateTime));

            return result;
        }

        public string RequestProperMessage(HttpActionExecutedContext actionExecutedContext)
        {
            string result = "";
            if (actionExecutedContext.Request.Method == HttpMethod.Get)
            {
                result = "مشاهده یک یا لیستی از آیتم ها";
            }
            else if (actionExecutedContext.Request.Method == HttpMethod.Post)
            {
                result = "افزودن یک آیتم جدید";
            }
            else if (actionExecutedContext.Request.Method == HttpMethod.Put)
            {
                result = "تغییر یک آیتم";
            }
            else if (actionExecutedContext.Request.Method == HttpMethod.Delete)
            {
                result = "حذف یک آیتم";
            }

            return result;
        }

        public string ResponseProperMessage(HttpActionExecutedContext actionExecutedContext)
        {
            string result = "";
            if (actionExecutedContext.Response.IsSuccessStatusCode == true &&
                actionExecutedContext.Request.Method == HttpMethod.Get)
            {
                result =  "نمایش آیتم(ها) با موفقیت انجام شد";
            }
            else if (actionExecutedContext.Response.IsSuccessStatusCode == false &&
                actionExecutedContext.Request.Method == HttpMethod.Get)
            {
                result =  "خطا در نمایش آیتم(ها)";
            }

            if (actionExecutedContext.Response.IsSuccessStatusCode == true &&
                actionExecutedContext.Response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                result =  "آیتم با موفقیت اضافه شد";
            }
            else if (actionExecutedContext.Response.IsSuccessStatusCode == false &&
                actionExecutedContext.Response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                result =  "خطا در افزودن آیتم";
            }

            if (actionExecutedContext.Response.IsSuccessStatusCode == true &&
                actionExecutedContext.Request.Method == HttpMethod.Put)
            {
                result =  "آیتم با موفقیت تغییر داده شد";
            }
            else if (actionExecutedContext.Response.IsSuccessStatusCode == false &&
                actionExecutedContext.Request.Method == HttpMethod.Put)
            {
                result =  "خطا در تغییر آیتم";
            }

            if (actionExecutedContext.Response.IsSuccessStatusCode == true &&
                actionExecutedContext.Request.Method == HttpMethod.Delete)
            {
                result =  "آیتم با موفقیت حذف شد";
            }
            else if (actionExecutedContext.Response.IsSuccessStatusCode == false &&
                actionExecutedContext.Request.Method == HttpMethod.Delete)
            {
                result =  "خطا در حذف آیتم";
            }


            return result;
        }

        public bool IsUserInRole(ApplicationDbContext db, string userId, string userRole = "Administrator")
        {
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);

            return userManager.IsInRole(userId, userRole);
        }

        public string LoadSettingFromWebConfig(string settingName)
        {
            return System.Web.Configuration.WebConfigurationManager.AppSettings[settingName];
        }

        public void SaveSettingInWebConfig(string settingName, string settingvalue)
        {
            System.Web.Configuration.WebConfigurationManager.AppSettings[settingName] = settingvalue;
        }

        //public List<T> ReCreateItemsWithPersianDates<T>(List<T> items)
        //{

        //    if (typeof(T) == typeof(IncomingCall))
        //    {
        //        foreach (var item in items)
        //        {
        //            item.Date = this.ConvertDateTimeToPersian(item.Date);
        //        }
        //    }



        //    return items;
        //}


    }
}