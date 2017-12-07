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

namespace se_CodeFirst_3.Controllers
{
    public class UsersController : Controller
    {

        ConnectToWebApiHelper helper = new ConnectToWebApiHelper();
        string basePath = "/api/users/";
        public UsersController()
        {
            basePath = "/api/users/";
        }

        // GET: Users
        public async Task<ActionResult> Index()
        {
            List<ApplicationUser> users = await helper.GetListOfItems<ApplicationUser>(basePath);

            return View(users);
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
        public ActionResult Create()
        {
            var db = new ApplicationDbContext();
            var roleStore = new RoleStore<IdentityRole>(db);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            ViewBag.Role = new SelectList(roleManager.Roles, "Name", "Name");

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
        public ActionResult Create([Bind(Include = "Email,Password,UserName,Salary,Benefits,Role")] RegisterBindingModel registerBindingModel)
        {
            if (ModelState.IsValid)
            {
                helper.CreateItem<RegisterBindingModel>("/api/account/register", registerBindingModel);
                return RedirectToAction("Index");
            }

            return View(registerBindingModel);
        }

        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = await helper.GetItem<ApplicationUser>(basePath + id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Salary,AbsentDays,OverTime,Benefits")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                helper.ChangeItem<ApplicationUser>(basePath + applicationUser.Id, applicationUser);
                return RedirectToAction("Index");
            }
            return View(applicationUser);
        }

        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = await helper.GetItem<ApplicationUser>(basePath + id);
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
        public ActionResult DeleteConfirmed(string id)
        {
            helper.DeleteItem(basePath, id);
            return RedirectToAction("Index");
        }

    }
}
