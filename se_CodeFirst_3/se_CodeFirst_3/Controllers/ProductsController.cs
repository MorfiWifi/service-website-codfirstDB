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
using se_CodeFirst_3.Filters;

namespace se_CodeFirst_3.Controllers
{
#if DEBUG

#else
    [RedirectIfNotAuthorized]
#endif
    public class ProductsController : Controller
    {
        ConnectToWebApiHelper helper = new ConnectToWebApiHelper();
        NotificationProviderHelper notificationHelper;

        string basePath = "api/products/";
        public ProductsController()
        {
            basePath = "api/products/";
            notificationHelper = new NotificationProviderHelper(this);
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
            ViewBag.suppliersList = await helper.GetListOfItems<Supplier>("api/Suppliers/");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,BuyUnitPrice,SellUnitPrice,UnitsInStock,UnitsOnOrder,SupplierId")] Product product, bool? stayOnCreatePage)
        {
            bool castedStayOnCreatePage = stayOnCreatePage.HasValue ? stayOnCreatePage.Value : false;
            if (ModelState.IsValid)
            {
                //product.Supplier = await helper.GetItem<Supplier>("api/suppliers/" + product.Supplier.Id);
                helper.CreateItem<Product>(basePath, product);
                notificationHelper.SuccessfulInsert(product.Name);
                if (castedStayOnCreatePage == true)
                {
                    return RedirectToAction("Create");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }

            ViewBag.SupplierId = new SelectList(await helper.GetListOfItems<Supplier>("api/suppliers"), "Id", "CompanyName", product.SupplierId);
            notificationHelper.FailureInsert(product.Name);
            ViewBag.suppliersList = await helper.GetListOfItems<Supplier>("api/Suppliers/");
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
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,BuyUnitPrice,SellUnitPrice,UnitsInStock,UnitsOnOrder,SupplierId")] Product product)
        {
            if (ModelState.IsValid)
            {
                helper.ChangeItem<Product>(basePath + product.Id, product);
                notificationHelper.SuccessfulChange(product.Name);
                return RedirectToAction("Index");
            }

            ViewBag.SupplierId = new SelectList(await helper.GetListOfItems<Supplier>("api/suppliers"), "Id", "CompanyName", product.SupplierId);
            notificationHelper.FailureChange(product.Name);
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
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            notificationHelper.SuccessfulDelete((await helper.GetItem<Product>(basePath + id)).Name);
            helper.DeleteItem(basePath, id);
            return RedirectToAction("Index");
        }

    }
}
