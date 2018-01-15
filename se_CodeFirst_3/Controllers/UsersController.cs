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
using PagedList;

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
        public async Task<ActionResult> Index(int? page, bool? searching, UserDTO userDTO)
        {
            bool castedSearching = searching.HasValue ? searching.Value : false;

            List<ApplicationUser> users = new List<ApplicationUser>();

            if (userDTO != null)
            {
                users = await helper.GetListOfItems<ApplicationUser>("api/users",
                    "?Email=" + userDTO.Email + "&" +
                    "UserName=" + userDTO.UserName + "&" +
                    "MinSalary=" + userDTO.MinSalary + "&" +
                    "MaxSalary=" + userDTO.MaxSalary + "&" +
                    "MinOvertime=" + userDTO.MinOvertime + "&" +
                    "MaxOvertime=" + userDTO.MaxOvertime + "&" +
                    "MinBenefits=" + userDTO.MinBenefits + "&" +
                    "MaxBenefits=" + userDTO.MaxBenefits + "&" +
                    "MinAbsentDays=" + userDTO.MinAbsentDays + "&" +
                    "MaxAbsentDays=" + userDTO.MaxAbsentDays
                    );
            }
            else
            {
                users = await helper.GetListOfItems<ApplicationUser>(basePath);
            }

            if (castedSearching)
            {
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return Json(users.ToPagedList(pageNumber, pageSize), JsonRequestBehavior.AllowGet);
            }
            else
            {
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(users.ToPagedList(pageNumber, pageSize));
            }
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
            //ViewBag.Role = new SelectList(await helper.GetListOfItems<IdentityRole>("api/roles/"), "Name", "Name");

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
        public async Task<ActionResult> Create([Bind(Include = "Email,Password,UserName,Salary,Benefits")] RegisterBindingModel registerBindingModel, bool? stayOnCreatePage)
        {
            bool castedStayOnCreatePage = stayOnCreatePage.HasValue ? stayOnCreatePage.Value : false;
            if (ModelState.IsValid)
            {
                var a = helper.CreateItem<RegisterBindingModel>("/api/account/register", registerBindingModel);
                notificationHelper.SuccessfulInsert(registerBindingModel.Email);
                //var itemCreated = helper.CreateItem<RegisterBindingModel>("/api/account/register", registerBindingModel);
                //if (itemCreated != null)
                //{
                //    notificationHelper.SuccessfulInsert(registerBindingModel.Email);
                //}
                //else
                //{
                //    notificationHelper.FailureInsert(registerBindingModel.Email);
                //}

                if (castedStayOnCreatePage == true)
                {
                    return RedirectToAction("Create");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }

            //ViewBag.Role = new SelectList(await helper.GetListOfItems<IdentityRole>("api/roles/"), "Name", "Name");
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

            //ViewBag.Role = new SelectList(await helper.GetListOfItems<IdentityRole>("api/roles/"), "Name", "Name");
            return View(registerBindingModel);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //"Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Salary,AbsentDays,OverTime,Benefits")
        public async Task<ActionResult> Edit([Bind(Include = "Id,Email,UserName,Password,Salary,AbsentDays,OverTime,Benefits")] RegisterBindingModel registerBindingModel)
        {
            if (ModelState.IsValid)
            {
                helper.ChangeItem<RegisterBindingModel>(basePath + registerBindingModel.Id, registerBindingModel);
                notificationHelper.SuccessfulChange(registerBindingModel.Email);
                return RedirectToAction("Index");
            }

            //ViewBag.Role = new SelectList(await helper.GetListOfItems<IdentityRole>("api/roles/"), "Name", "Name");
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
            string deletedItem = (await helper.GetItem<UserListViewModel>(basePath + id)).UserName;
            bool successfulDelete = helper.DeleteItem(basePath, id);
            if (successfulDelete)
                notificationHelper.SuccessfulDelete(deletedItem);
            else
                notificationHelper.CustomFailureMessage("خطا در حذف " + deletedItem);

            return RedirectToAction("Index");
        }

    }
}
