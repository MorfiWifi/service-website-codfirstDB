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
    public class OrdersController : Controller
    {
        ConnectToWebApiHelper helper = new ConnectToWebApiHelper();
        NotificationProviderHelper notificationHelper;
        UsefulMethodsHelper methodHelper;

        string basePath = "api/orders/";
        public OrdersController()
        {
            basePath = "api/orders/";
            notificationHelper = new NotificationProviderHelper(this);
            methodHelper = new UsefulMethodsHelper();
        }

        // GET: Orders
        public async Task<ActionResult> Index()
        {
            List<Order> orders = await helper.GetListOfItems<Order>(basePath);

            //using PersianDates::
            foreach (var item in orders)
            {
                item.OrderDate = methodHelper.ConvertDateTimeToPersian(item.OrderDate);
                item.RequiredDate = methodHelper.ConvertDateTimeToPersian(item.RequiredDate);
            }

            return View(orders);
        }

        // GET: Orders/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await helper.GetItem<Order>(basePath + id);
            if (order == null)
            {
                return HttpNotFound();
            }

            //using PersianDates::
            order.OrderDate = methodHelper.ConvertDateTimeToPersian(order.OrderDate);
            order.RequiredDate = methodHelper.ConvertDateTimeToPersian(order.RequiredDate);


            return View(order);
        }

        // GET: Orders/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.ContractId = new SelectList(await helper.GetListOfItems<Contract>("api/contracts/"), "Id", "Content");
            ViewBag.CustomerId = new SelectList(await helper.GetListOfItems<Customer>("api/customers/"), "Id", "Name");

            ViewBag.ContractsList = await helper.GetListOfItems<Contract>("api/contracts/");
            ViewBag.CustomersList = await helper.GetListOfItems<Customer>("api/customers/");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,OrderDate,RequiredDate,CustomerId,ContractId")] Order order, bool? stayOnCreatePage)
        {
            bool castedStayOnCreatePage = stayOnCreatePage.HasValue ? stayOnCreatePage.Value : false;

            var convertedOrderDateTime = methodHelper.ConvertDateTimeToGregorian(order.OrderDate);
            var convertedRequiredDateTime = methodHelper.ConvertDateTimeToGregorian(order.RequiredDate);

            order.OrderDate = convertedOrderDateTime;
            order.RequiredDate = convertedRequiredDateTime;


            if (ModelState.IsValid)
            {
                helper.CreateItem<Order>(basePath, order);
                notificationHelper.SuccessfulInsert(order.Id.ToString());
                if (castedStayOnCreatePage == true)
                {
                    return RedirectToAction("Create");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }

            ViewBag.ContractId = new SelectList(await helper.GetListOfItems<Contract>("api/contracts/"), "Id", "Content");
            ViewBag.CustomerId = new SelectList(await helper.GetListOfItems<Customer>("api/customers/"), "Id", "Name");

            ViewBag.ContractsList = await helper.GetListOfItems<Contract>("api/contracts/");
            ViewBag.CustomersList = await helper.GetListOfItems<Customer>("api/customers/");

            notificationHelper.FailureInsert(order.Id.ToString());
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await helper.GetItem<Order>(basePath + id);
            if (order == null)
            {
                return HttpNotFound();
            }

            order.OrderDate = methodHelper.ConvertDateTimeToPersian(order.OrderDate);
            order.RequiredDate = methodHelper.ConvertDateTimeToPersian(order.RequiredDate);

            ViewBag.ContractId = new SelectList(await helper.GetListOfItems<Contract>("api/contracts/"), "Id", "Content");
            ViewBag.CustomerId = new SelectList(await helper.GetListOfItems<Customer>("api/customers/"), "Id", "Name");
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,OrderDate,RequiredDate,CustomerId,ContractId")] Order order)
        {
            if (ModelState.IsValid)
            {
                helper.ChangeItem<Order>(basePath + order.Id, order);
                notificationHelper.SuccessfulChange(order.Id.ToString());
                return RedirectToAction("Index");
            }
            ViewBag.ContractId = new SelectList(await helper.GetListOfItems<Contract>("api/contracts/"), "Id", "Content");
            ViewBag.CustomerId = new SelectList(await helper.GetListOfItems<Customer>("api/customers/"), "Id", "Name");

            notificationHelper.FailureChange(order.Id.ToString());
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await helper.GetItem<Order>(basePath + id);
            if (order == null)
            {
                return HttpNotFound();
            }

            //using PersianDates::
            order.OrderDate = methodHelper.ConvertDateTimeToPersian(order.OrderDate);
            order.RequiredDate = methodHelper.ConvertDateTimeToPersian(order.RequiredDate);

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            notificationHelper.SuccessfulDelete((await helper.GetItem<Order>(basePath + id)).Id.ToString());
            helper.DeleteItem(basePath, id);
            return RedirectToAction("Index");
        }

    }
}
