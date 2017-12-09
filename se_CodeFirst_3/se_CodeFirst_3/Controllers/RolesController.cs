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
using Microsoft.AspNet.Identity.EntityFramework;
using se_CodeFirst_3.Filters;

namespace se_CodeFirst_3.Controllers
{
#if DEBUG

#else
    [RedirectIfNotAuthorized]
#endif
    public class RolesController : Controller
    {
        ConnectToWebApiHelper helper = new ConnectToWebApiHelper();
        NotificationProviderHelper notificationHelper;

        string basePath = "api/roles/";
        public RolesController()
        {
            basePath = "api/roles/";
            notificationHelper = new NotificationProviderHelper(this);
        }

        // GET: Roles
        public async Task<ActionResult> Index()
        {
            List<IdentityRole> roles = await helper.GetListOfItems<IdentityRole>(basePath);

            return View(roles);
        }

        // GET: Roles/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IdentityRole role = await helper.GetItem<IdentityRole>(basePath + id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] IdentityRole identityRole, bool? stayOnCreatePage)
        {
            bool castedStayOnCreatePage = stayOnCreatePage.HasValue ? stayOnCreatePage.Value : false;
            if (ModelState.IsValid)
            {
                helper.CreateItem<IdentityRole>(basePath, identityRole);
                notificationHelper.SuccessfulInsert(identityRole.Name);
                if (castedStayOnCreatePage == true)
                {
                    return RedirectToAction("Create");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }

            notificationHelper.FailureInsert(identityRole.Name);
            return View(identityRole);
        }

        // GET: Roles/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IdentityRole role = await helper.GetItem<IdentityRole>(basePath + id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] IdentityRole identityRole)
        {
            if (ModelState.IsValid)
            {
                helper.ChangeItem<IdentityRole>(basePath + identityRole.Id, identityRole);
                notificationHelper.SuccessfulChange(identityRole.Name);
                return RedirectToAction("Index");
            }

            notificationHelper.FailureChange(identityRole.Name);
            return View(identityRole);
        }

        // GET: Roles/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IdentityRole role = await helper.GetItem<IdentityRole>(basePath + id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            notificationHelper.SuccessfulDelete((await helper.GetItem<IdentityRole>(basePath + id)).Name);
            helper.DeleteItem(basePath, id);
            return RedirectToAction("Index");
        }

    }
}
