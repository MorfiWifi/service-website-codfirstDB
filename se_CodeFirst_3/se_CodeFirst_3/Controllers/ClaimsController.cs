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
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using se_CodeFirst_3.Helper;
using System.Security.Claims;
using se_CodeFirst_3.Filters;

namespace se_CodeFirst_3.Controllers
{
#if DEBUG

#else
    [RedirectIfNotAuthorized]
#endif
    public class ClaimsController : Controller
    {
        string basePath = "api/claims/";
        ConnectToWebApiHelper helper;
        NotificationProviderHelper notificationHelper;

        public ClaimsController()
        {
            basePath = "api/claims/";
            helper = new ConnectToWebApiHelper();
            notificationHelper = new NotificationProviderHelper(this);
        }

        // GET: Claims
        public async Task<ActionResult> Index()
        {
            return View(await helper.GetListOfItems<ApplicationUser>("api/users/"));
        }

        // GET: Claims/Details/5
        //public async Task<ActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ClaimViewModel claimViewModel = await db.ClaimViewModels.FindAsync(id);
        //    if (claimViewModel == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(claimViewModel);
        //}

        // GET: Claims/Create
        public async Task<ActionResult> Create(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await helper.GetItem<ApplicationUser>("api/UsersA/" + id);
            ViewBag.ClaimsBefore = user.Claims.ToList();
            ViewBag.Claims = await helper.GetListOfItems<ClaimViewModel>("api/ClaimsA");

            return View();
        }

        // POST: Claims/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Route("Claims/Create/{id}/{claimId}")]
        public async Task<ActionResult> Create([Bind(Include = "Id,UserName,Claims,claimId")]string id, int claimId)
        {
            var user = await helper.GetItem<ApplicationUser>("api/UsersA/" + id);

            ViewBag.ClaimsBefore = user.Claims.ToList();
            ViewBag.Claims = (await helper.GetListOfItems<ClaimViewModel>("api/claims")).ToList();

            if (ModelState.IsValid)
            {

                helper.CreateItem<ApplicationUser>("api/Claims/" + id + "/" + claimId, null);

                notificationHelper.CustomSuccessfulMessage("سطح دسترسی با موفقیت تغییر داده شد");

                return View(user);
            }

            notificationHelper.CustomFailureMessage("خطا در تغییر سطح دسترسی");
            return View(user);
        }

        // GET: Claims/Edit/5
        //public async Task<ActionResult> Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var user = db.Users.Find(id);
        //    if (user == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(user);
        //}

        // POST: Claims/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "Id,UserName,Claims")] ApplicationUser user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(user).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    return View(user);
        //}

        //// GET: Claims/Delete/5
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ClaimViewModel claimViewModel = await db.ClaimViewModels.FindAsync(id);
        //    if (claimViewModel == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(claimViewModel);
        //}

        //// POST: Claims/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    ClaimViewModel claimViewModel = await db.ClaimViewModels.FindAsync(id);
        //    db.ClaimViewModels.Remove(claimViewModel);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
