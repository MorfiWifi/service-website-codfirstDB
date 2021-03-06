﻿using System;
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
using PagedList;

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
        public async Task<ActionResult> Index(bool? includeDeletedItems, int? page, bool? searching, Supplier supplier)
        {
            bool castedIncludeDeletedItems = includeDeletedItems.HasValue ? includeDeletedItems.Value : false;
            bool castedSearching = searching.HasValue ? searching.Value : false;

            List<Supplier> suppliers = new List<Supplier>();

            if (supplier != null)
            {
                suppliers = await helper.GetListOfItems<Supplier>("api/suppliers",
                    "?CompanyName=" + supplier.CompanyName + "&" +
                    "Name=" + supplier.Name + "&" +
                    "Address=" + supplier.Address + "&" +
                    "PhoneNumber=" + supplier.PhoneNumber
                    );
            }
            else if (castedIncludeDeletedItems == false)
            {
                suppliers = await helper.GetListOfItems<Supplier>(basePath);
            }
            else
            {
                suppliers = await helper.GetListOfItems<Supplier>("api/allSuppliers");
            }

            if (castedSearching)
            {
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return Json(suppliers.ToPagedList(pageNumber, pageSize), JsonRequestBehavior.AllowGet);
            }
            else
            {
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(suppliers.ToPagedList(pageNumber, pageSize));
            }

        }

        //[Route("Suppliers/SearchItems")]
        //public async Task<ActionResult> SearchItems(Supplier supplier)
        //{

        //}

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
                var itemCreated = helper.CreateItem<Supplier>(basePath, supplier);
                if (itemCreated != null)
                {
                    notificationHelper.SuccessfulInsert(supplier.Name);
                }
                else
                {
                    notificationHelper.FailureInsert(supplier.Name);
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
                var itemEdited = helper.ChangeItem<Supplier>(basePath + supplier.Id, supplier);
                if (itemEdited != null)
                    notificationHelper.SuccessfulChange(supplier.Name);
                else
                    notificationHelper.FailureChange(supplier.Name);

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
            string deletedItem = (await helper.GetItem<Supplier>(basePath + id)).Name;
            bool successfulDelete = helper.DeleteItem(basePath, id);
            if (successfulDelete)
                notificationHelper.SuccessfulDelete(deletedItem);
            else
                notificationHelper.CustomFailureMessage("خطا در حذف " + deletedItem);

            return RedirectToAction("Index");
        }

        //public async Task<ActionResult> Delete()
        //{
        //    List<Supplier> suppliers = new List<Supplier>();
        //    foreach (var item in idOfItemsForMultipleDelete.Split(','))
        //    {
        //        Supplier supplier = await helper.GetItem<Supplier>(basePath + item);
        //        if (supplier == null)
        //        {
        //            return HttpNotFound();
        //        }
        //        suppliers.Add(supplier);
        //    }

        //    //we want to use the below id in post method later:
        //    ViewBag.SuppliersIds = idOfItemsForMultipleDelete;
        //    return View(suppliers);
        //}

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //[Route("Suppliers/DeleteMultiple/{idOfItemsForMultipleDelete}")]
        //public async Task<ActionResult> DeleteConfirmed(string idOfItemsForMultipleDelete)
        //{
        //    var a = idOfItemsForMultipleDelete;
        //    return RedirectToAction("Index");
        //    //notificationHelper.SuccessfulDelete((await helper.GetItem<Supplier>(basePath + id)).Name);
        //    //helper.DeleteItem(basePath, id);
        //    //return RedirectToAction("Index");
        //}

    }
}
