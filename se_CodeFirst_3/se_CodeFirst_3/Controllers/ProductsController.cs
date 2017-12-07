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

namespace se_CodeFirst_3.Controllers
{
    public class ProductsController : Controller
    {
        ConnectToWebApiHelper helper = new ConnectToWebApiHelper();

        string basePath = "api/products/";
        public ProductsController()
        {
            basePath = "api/products/";
        }

        // GET: Products
        public async Task<ActionResult> Index()
        {
            List<Product> products = await helper.GetListOfItems<Product>(basePath);

            return View(products);
        }

        // GET: Products/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await helper.GetItem<Product>(basePath + id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.SupplierId = new SelectList(await helper.GetListOfItems<Supplier>("api/suppliers"), "Id", "CompanyName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,UnitPrice,UnitsInStock,UnitsOnOrder,SupplierId")] Product product)//, int supplierId)//
        {
            if (ModelState.IsValid)
            {
                //product.Supplier = await helper.GetItem<Supplier>("api/suppliers/" + product.Supplier.Id);
                helper.CreateItem<Product>(basePath, product);
                return RedirectToAction("Index");
            }

            ViewBag.SupplierId = new SelectList(await helper.GetListOfItems<Supplier>("api/suppliers"), "Id", "CompanyName", product.SupplierId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await helper.GetItem<Product>(basePath + id);
            if (product == null)
            {
                return HttpNotFound();
            }

            ViewBag.SupplierId = new SelectList(await helper.GetListOfItems<Supplier>("api/suppliers"), "Id", "CompanyName", product.SupplierId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,UnitPrice,UnitsInStock,UnitsOnOrder,SupplierId")] Product product)
        {
            if (ModelState.IsValid)
            {
                helper.ChangeItem<Product>(basePath + product.Id, product);
                return RedirectToAction("Index");
            }

            ViewBag.SupplierId = new SelectList(await helper.GetListOfItems<Supplier>("api/suppliers"), "Id", "CompanyName", product.SupplierId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await helper.GetItem<Product>(basePath + id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            helper.DeleteItem(basePath, id);
            return RedirectToAction("Index");
        }

    }
}
