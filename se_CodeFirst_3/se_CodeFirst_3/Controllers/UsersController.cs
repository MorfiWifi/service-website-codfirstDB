using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using se_CodeFirst_3.Models;
using se_CodeFirst_3.Helper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using se_CodeFirst_3.Filters;

namespace se_CodeFirst_3.Controllers
{
#if DEBUG

#else
    [RedirectIfNotAuthorized]
#endif
    public class UsersController : Controller
    {

        ConnectToWebApiHelper helper = new ConnectToWebApiHelper();
        NotificationProviderHelper notificationHelper;

        string basePath = "/api/users/";
        public UsersController()
        {
            basePath = "/api/users/";
            notificationHelper = new NotificationProviderHelper(this);
        }

        // GET: Users
        public async Task<ActionResult> Index()
        {
            List<ApplicationUser> users = await helper.GetListOfItems<ApplicationUser>(basePath);
            List<UserListViewModel> users2 = new List<UserListViewModel>();

            foreach (var item in users)
            {
                var role = new IdentityRole();
                if (item.Roles.Count > 0)
                {
                    role = await helper.GetItem<IdentityRole>("api/roles/" + item.Roles.FirstOrDefault().RoleId);
                }
                users2.Add(new UserListViewModel
                {
                    Id = item.Id,
                    Email = item.Email,
                    UserName = item.UserName,
                    AbsentDays = item.AbsentDays,
                    Salary = item.Salary,
                    Benefits = item.Benefits,
                    OverTime = item.OverTime,
                    Role = role.Name
                });
            }
            return View(users2);
        }

        // GET: Users/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserViewModel userInformation = await helper.GetItem<UserViewModel>(basePath + id);
            if (userInformation == null)
            {
                return HttpNotFound();
            }
            return View(userInformation);
        }

        // GET: Users/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.Role = new SelectList(await helper.GetListOfItems<IdentityRole>("api/roles/"), "Name", "Name");

            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Salary,AbsentDays,OverTime,Benefits")] ApplicationUser applicationUser)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        helper.CreateItem<ApplicationUser>("/api/account/register", applicationUser);
        //        return RedirectToAction("Index");
        //    }

        //    return View(applicationUser);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Email,Password,UserName,Salary,Benefits,Role")] RegisterBindingModel registerBindingModel, bool? stayOnCreatePage)
        {
            bool castedStayOnCreatePage = stayOnCreatePage.HasValue ? stayOnCreatePage.Value : false;
            if (ModelState.IsValid)
            {
                helper.CreateItem<RegisterBindingModel>("/api/account/register", registerBindingModel);
                notificationHelper.SuccessfulInsert(registerBindingModel.Email);
                if (castedStayOnCreatePage == true)
                {
                    return RedirectToAction("Create");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }

            ViewBag.Role = new SelectList(await helper.GetListOfItems<IdentityRole>("api/roles/"), "Name", "Name");
            notificationHelper.FailureInsert(registerBindingModel.Email);
            return View(registerBindingModel);
        }

        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegisterBindingModel registerBindingModel = await helper.GetItem<RegisterBindingModel>(basePath + id);
            if (registerBindingModel == null)
            {
                return HttpNotFound();
            }

            ViewBag.Role = new SelectList(await helper.GetListOfItems<IdentityRole>("api/roles/"), "Name", "Name");
            return View(registerBindingModel);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //"Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Salary,AbsentDays,OverTime,Benefits")
        public async Task<ActionResult> Edit([Bind(Include = "Id,Email,UserName,Password,Salary,AbsentDays,OverTime,Benefits,Role")] RegisterBindingModel registerBindingModel)
        {
            if (ModelState.IsValid)
            {
                helper.ChangeItem<RegisterBindingModel>(basePath + registerBindingModel.Id, registerBindingModel);
                notificationHelper.SuccessfulChange(registerBindingModel.Email);
                return RedirectToAction("Index");
            }

            ViewBag.Role = new SelectList(await helper.GetListOfItems<IdentityRole>("api/roles/"), "Name", "Name");
            notificationHelper.FailureChange(registerBindingModel.Email);
            return View(registerBindingModel);
        }

        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserListViewModel applicationUser = await helper.GetItem<UserListViewModel>(basePath + id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = id;
            return View(applicationUser);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            notificationHelper.SuccessfulDelete((await helper.GetItem<UserListViewModel>(basePath + id)).UserName);
            helper.DeleteItem(basePath, id);
            return RedirectToAction("Index");
        }

    }
}
