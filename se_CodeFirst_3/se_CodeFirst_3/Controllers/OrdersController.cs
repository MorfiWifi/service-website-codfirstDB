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
    public class OrdersController : Controller
    {
        ConnectToWebApiHelper helper = new ConnectToWebApiHelper();

        string basePath = "api/orders/";
        public OrdersController()
        {
            basePath = "api/orders/";
        }

        // GET: Orders
        public async Task<ActionResult> Index()
        {
            List<Order> orders = await helper.GetListOfItems<Order>(basePath);

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
            return View(order);
        }

        // GET: Orders/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.ContractId = new SelectList(await helper.GetListOfItems<Contract>("api/contracts/"), "Id", "Content");
            ViewBag.CustomerId = new SelectList(await helper.GetListOfItems<Customer>("api/customers/"), "Id", "Name");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,OrderDate,RequiredDate,CustomerId,ContractId")] Order order)
        {
            if (ModelState.IsValid)
            {
                helper.CreateItem<Order>(basePath, order);
                return RedirectToAction("Index");
            }

            ViewBag.ContractId = new SelectList(await helper.GetListOfItems<Contract>("api/contracts/"), "Id", "Content");
            ViewBag.CustomerId = new SelectList(await helper.GetListOfItems<Customer>("api/customers/"), "Id", "Name");
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
                return RedirectToAction("Index");
            }
            ViewBag.ContractId = new SelectList(await helper.GetListOfItems<Contract>("api/contracts/"), "Id", "Content");
            ViewBag.CustomerId = new SelectList(await helper.GetListOfItems<Customer>("api/customers/"), "Id", "Name");
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
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            helper.DeleteItem(basePath, id);

            return RedirectToAction("Index");
        }

    }
}
