using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using se_CodeFirst_3.Models;
using se_CodeFirst_3.Helper;
using System.Threading.Tasks;

namespace se_CodeFirst_3.Controllers
{
    public class SuppliersController : Controller
    {
        ConnectToWebApiHelper helper = new ConnectToWebApiHelper();
        string basePath = "api/suppliers/";
        public SuppliersController()
        {
            basePath = "api/suppliers/";
        }

        // GET: Suppliers
        public async Task<ActionResult> Index()
        {
            List<Supplier> suppliers = await helper.GetListOfItems<Supplier>(basePath);

            return View(suppliers);
        }

        // GET: Suppliers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = await helper.GetItem<Supplier>(basePath + id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // GET: Suppliers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Suppliers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CompanyName,Name,Address,Phone")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                helper.CreateItem<Supplier>(basePath, supplier);
                return RedirectToAction("Index");
            }

            return View(supplier);
        }

        // GET: Suppliers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = await helper.GetItem<Supplier>(basePath + id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // POST: Suppliers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CompanyName,Name,Address,Phone")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                helper.ChangeItem<Supplier>(basePath + supplier.Id, supplier);
                return RedirectToAction("Index");
            }
            return View(supplier);
        }

        // GET: Suppliers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = await helper.GetItem<Supplier>(basePath + id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            helper.DeleteItem(basePath, id);
            return RedirectToAction("Index");
        }

    }
}
