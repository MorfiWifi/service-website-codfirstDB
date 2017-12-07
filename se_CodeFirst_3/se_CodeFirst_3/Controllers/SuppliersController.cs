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
using se_CodeFirst_3.Filters;

namespace se_CodeFirst_3.Controllers
{
#if DEBUG

#else
    [RedirectIfNotAuthorized]
#endif
    public class SuppliersController : Controller
    {
        ConnectToWebApiHelper helper = new ConnectToWebApiHelper();
        NotificationProviderHelper notificationHelper;

        string basePath = "api/suppliers/";
        public SuppliersController()
        {
            basePath = "api/suppliers/";
            notificationHelper = new NotificationProviderHelper(this);
        }

        // GET: Suppliers
        public async Task<ActionResult> Index(bool? includeDeletedItems)
        {
            bool castedIncludeDeletedItems = includeDeletedItems.HasValue ? includeDeletedItems.Value : false;
            if (castedIncludeDeletedItems)
            {
                List<Supplier> suppliers = await helper.GetListOfItems<Supplier>("api/allSuppliers");

                return View(suppliers);
            }
            else
            {
                List<Supplier> suppliers = await helper.GetListOfItems<Supplier>(basePath);

                return View(suppliers);
            }
        }

        // GET: Suppliers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Supplier supplier = await helper.GetItem<Supplier>(basePath + id);
            //we want to share more details about Supplier, so instead of above commented code, we create an instance of SupplierInformationViewModel: 
            SupplierInformationViewModel supplierInformation = await helper.GetItem<SupplierInformationViewModel>(basePath + id);

            if (supplierInformation == null)
            {
                return HttpNotFound();
            }
            return View(supplierInformation);


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
        public ActionResult Create([Bind(Include = "Id,CompanyName,Name,Address,PhoneNumber")] Supplier supplier, bool? stayOnCreatePage)
        {
            bool castedStayOnCreatePage = stayOnCreatePage.HasValue ? stayOnCreatePage.Value : false;
            //bool castedStayOnCreatePageAndKeepInputsDatas = stayOnCreatePageAndKeepInputsDatas.HasValue ? stayOnCreatePageAndKeepInputsDatas.Value : false;

            if (ModelState.IsValid)
            {
                helper.CreateItem<Supplier>(basePath, supplier);
                notificationHelper.SuccessfulInsert(supplier.Name);
                if (castedStayOnCreatePage == true)
                {
                    return RedirectToAction("Create");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }

            notificationHelper.FailureInsert(supplier.Name);
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
        public ActionResult Edit([Bind(Include = "Id,CompanyName,Name,Address,PhoneNumber,IsDeleted")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                helper.ChangeItem<Supplier>(basePath + supplier.Id, supplier);
                notificationHelper.SuccessfulChange(supplier.Name);
                return RedirectToAction("Index");
            }
            notificationHelper.FailureChange(supplier.Name);
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

            //we want to use the below id in post method later:
            ViewBag.SupplierId = id;
            return View(supplier);
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            notificationHelper.SuccessfulDelete((await helper.GetItem<Supplier>(basePath + id)).Name);
            helper.DeleteItem(basePath, id);
            return RedirectToAction("Index");
        }

    }
}
