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

namespace se_CodeFirst_3.Controllers
{
    public class Order_DetailController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Order_Detail
        public async Task<ActionResult> Index()
        {
            return View(await db.Order_Details.ToListAsync());
        }

        // GET: Order_Detail/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order_Detail order_Detail = await db.Order_Details.FindAsync(id);
            if (order_Detail == null)
            {
                return HttpNotFound();
            }
            return View(order_Detail);
        }

        // GET: Order_Detail/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Order_Detail/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,UnitPrice,Quantity,Discount")] Order_Detail order_Detail)
        {
            if (ModelState.IsValid)
            {
                db.Order_Details.Add(order_Detail);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(order_Detail);
        }

        // GET: Order_Detail/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order_Detail order_Detail = await db.Order_Details.FindAsync(id);
            if (order_Detail == null)
            {
                return HttpNotFound();
            }
            return View(order_Detail);
        }

        // POST: Order_Detail/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,UnitPrice,Quantity,Discount")] Order_Detail order_Detail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order_Detail).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(order_Detail);
        }

        // GET: Order_Detail/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order_Detail order_Detail = await db.Order_Details.FindAsync(id);
            if (order_Detail == null)
            {
                return HttpNotFound();
            }
            return View(order_Detail);
        }

        // POST: Order_Detail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Order_Detail order_Detail = await db.Order_Details.FindAsync(id);
            db.Order_Details.Remove(order_Detail);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
