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
using PagedList;

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
        public async Task<ActionResult> Index(int? page, bool? searching, ProductDTO productDTO)
        {
            bool castedSearching = searching.HasValue ? searching.Value : false;

            List<Product> products = new List<Product>();

            if (productDTO != null)
            {
                products = await helper.GetListOfItems<Product>("api/products",
                    "?Name=" + productDTO.Name + "&" +
                    "SupplierCompanyName=" + productDTO.SupplierCompanyName + "&" +
                    "MinUnitsInStock=" + productDTO.MinUnitsInStock + "&" +
                    "MaxUnitsInStock=" + productDTO.MaxUnitsInStock + "&" +
                    "MinUnitPrice=" + productDTO.MinUnitPrice + "&" +
                    "MaxUnitPrice=" + productDTO.MaxUnitPrice + "&" +
                    "MinSellUnitPrice=" + productDTO.MinSellUnitPrice + "&" +
                    "MaxSellUnitPrice=" + productDTO.MaxSellUnitPrice
                    );
            }
            else
            {
                products = await helper.GetListOfItems<Product>(basePath);
            }

            if (castedSearching)
            {
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return Json(products.ToPagedList(pageNumber, pageSize), JsonRequestBehavior.AllowGet);
            }
            else
            {
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(products.ToPagedList(pageNumber, pageSize));
            }
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
            ViewBag.SuppliersList2 = await helper.GetListOfItems<Supplier>("api/Suppliers/");

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
                var itemCreated = helper.CreateItem<Product>(basePath, product);
                if (itemCreated != null)
                {
                    notificationHelper.SuccessfulInsert(product.Name);
                }
                else
                {
                    notificationHelper.FailureInsert(product.Name);
                }

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
            ViewBag.SuppliersList2 = await helper.GetListOfItems<Supplier>("api/Suppliers/");

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
                var itemEdited = helper.ChangeItem<Product>(basePath + product.Id, product);
                if (itemEdited != null)
                    notificationHelper.SuccessfulChange(product.Name);
                else
                    notificationHelper.FailureChange(product.Name);

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
            string deletedItem = (await helper.GetItem<Product>(basePath + id)).Name;
            bool successfulDelete = helper.DeleteItem(basePath, id);
            if (successfulDelete)
                notificationHelper.SuccessfulDelete(deletedItem);
            else
                notificationHelper.CustomFailureMessage("خطا در حذف " + deletedItem);

            return RedirectToAction("Index");
        }

    }
}
