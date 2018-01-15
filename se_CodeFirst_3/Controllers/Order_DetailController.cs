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
    [Route("Orders/{parentItemId}/Order_Detail/{action}/{id?}")]
    public class Order_DetailController : Controller
    {
        ConnectToWebApiHelper helper = new ConnectToWebApiHelper();
        NotificationProviderHelper notificationHelper;

        string basePath = "api/Order_Detail/";
        public Order_DetailController()
        {
            basePath = "api/Order_Detail/";
            notificationHelper = new NotificationProviderHelper(this);
        }

        // GET: Order_Detail
        public async Task<ActionResult> Index(int parentItemId, int? page, bool? searching, Order_DetailDTO order_detailDTO)
        {
            List<Order_Detail> order_details = new List<Order_Detail>();
            bool castedSearching = searching.HasValue ? searching.Value : false;
            
            ViewBag.ParantName = (await helper.GetItem<Order>("api/Orders/" + parentItemId)).Id;

            if (order_detailDTO != null)
            {
                order_details = await helper.GetListOfItems<Order_Detail>("api/Order_Detail",
                    "?Name=" + order_detailDTO.Name + "&" +
                    "MinQuantity=" + order_detailDTO.MinQuantity + "&" +
                    "MaxQuantity=" + order_detailDTO.MaxQuantity
                    );
            }
            else
            {
                order_details = await helper.GetListOfItems<Order_Detail>(basePath);
            }

            List<Order_Detail> Order_Details = (from item in order_details
                                                where item.OrderId == parentItemId
                                                select item).ToList();

            if (castedSearching)
            {
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return Json(Order_Details.ToPagedList(pageNumber, pageSize), JsonRequestBehavior.AllowGet);
            }
            else
            {
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(Order_Details.ToPagedList(pageNumber, pageSize));
            }

        }

        // GET: Order_Detail/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order_Detail order_Detail = await helper.GetItem<Order_Detail>(basePath + id);
            if (order_Detail == null)
            {
                return HttpNotFound();
            }
            return View(order_Detail);
        }

        // GET: Order_Detail/Create
        public async Task<ActionResult> Create(int parentItemId)
        {
            ViewBag.ProductId = new SelectList(await helper.GetListOfItems<Product>("api/products/"), "Id", "Name");
            return View();
        }

        // POST: Order_Detail/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Quantity,ProductId,OrderId")] Order_Detail order_Detail, bool? stayOnCreatePage, int parentItemId)
        {
            bool castedStayOnCreatePage = stayOnCreatePage.HasValue ? stayOnCreatePage.Value : false;

            if (ModelState.IsValid)
            {
                order_Detail.OrderId = parentItemId;
                Order_Detail od = helper.CreateItem<Order_Detail>(basePath, order_Detail);

                if (od == null)
                {
                    notificationHelper.CustomFailureMessage("تعداد کالاها نمی تواند از موجودی بیشتر باشد.");

                    ViewBag.ProductId = new SelectList(await helper.GetListOfItems<Product>("api/products/"), "Id", "Name");

                    return View(order_Detail);
                }

                notificationHelper.SuccessfulInsert(order_Detail.Id.ToString());
                if (castedStayOnCreatePage == true)
                {
                    return RedirectToAction("Create");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }

            ViewBag.ProductId = new SelectList(await helper.GetListOfItems<Product>("api/products/"), "Id", "Name", order_Detail.ProductId);

            notificationHelper.FailureInsert(order_Detail.Id.ToString());
            return View(order_Detail);
        }

        // GET: Order_Detail/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order_Detail order_Detail = await helper.GetItem<Order_Detail>(basePath + id);
            if (order_Detail == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(await helper.GetListOfItems<Product>("api/products/"), "Id", "Name", order_Detail.ProductId);
            return View(order_Detail);
        }

        // POST: Order_Detail/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Quantity,ProductId,OrderId")] Order_Detail order_Detail, int parentItemId)
        {
            if (ModelState.IsValid)
            {
                order_Detail.OrderId = parentItemId;
                Order_Detail od = helper.ChangeItem<Order_Detail>(basePath + order_Detail.Id, order_Detail);

                if (od == null)
                {
                    notificationHelper.CustomFailureMessage("تعداد کالاها نمی تواند از موجودی بیشتر باشد.");

                    ViewBag.ProductId = new SelectList(await helper.GetListOfItems<Product>("api/products/"), "Id", "Name", order_Detail.ProductId);

                    return View(order_Detail);
                }

                notificationHelper.SuccessfulChange(order_Detail.Id.ToString());
                return RedirectToAction("Index");
            }
            ViewBag.ProductId = new SelectList(await helper.GetListOfItems<Product>("api/products/"), "Id", "Name", order_Detail.ProductId);

            notificationHelper.FailureChange(order_Detail.Id.ToString());
            return View(order_Detail);
        }

        // GET: Order_Detail/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order_Detail order_Detail = await helper.GetItem<Order_Detail>(basePath + id);
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
            string deletedItem = (await helper.GetItem<Order_Detail>(basePath + id)).Id.ToString();
            bool successfulDelete = helper.DeleteItem(basePath, id);
            if (successfulDelete)
                notificationHelper.SuccessfulDelete(deletedItem);
            else
                notificationHelper.CustomFailureMessage("خطا در حذف " + deletedItem);

            return RedirectToAction("Index");
        }

    }
}
