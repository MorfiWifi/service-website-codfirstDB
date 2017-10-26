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
    public class Order_DetailController : Controller
    {
        ConnectToWebApiHelper helper = new ConnectToWebApiHelper();

        string basePath = "api/Order_Detail/";
        public Order_DetailController()
        {
            basePath = "api/Order_Detail/";
        }

        // GET: Order_Detail
        public async Task<ActionResult> Index()
        {
            //var order_Details = db.Order_Details.Include(o => o.Order).Include(o => o.Product);
            //return View(await order_Details.ToListAsync());
            List<Order_Detail> orders = await helper.GetListOfItems<Order_Detail>(basePath);

            return View(orders);
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
        public async Task<ActionResult> Create()
        {
            ViewBag.OrderId = new SelectList(await helper.GetListOfItems<Order>("api/orders/"), "Id", "Id");
            ViewBag.ProductId = new SelectList(await helper.GetListOfItems<Product>("api/products/"), "Id", "Name");
            return View();
        }

        // POST: Order_Detail/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Quantity,ProductId,OrderId")] Order_Detail order_Detail)
        {
            if (ModelState.IsValid)
            {
                Order_Detail od = helper.CreateItem<Order_Detail>(basePath, order_Detail);

                if (od == null)
                {
                    ViewBag.Title = "تعداد کالاها نمی تواند از موجودی بیشتر باشد.";
                }

                return RedirectToAction("Index");
            }

            ViewBag.OrderId = new SelectList(await helper.GetListOfItems<Order>("api/orders/"), "Id", "Id");
            ViewBag.ProductId = new SelectList(await helper.GetListOfItems<Product>("api/products/"), "Id", "Name");
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
            ViewBag.OrderId = new SelectList(await helper.GetListOfItems<Order>("api/orders/"), "Id", "Id");
            ViewBag.ProductId = new SelectList(await helper.GetListOfItems<Product>("api/products/"), "Id", "Name");
            return View(order_Detail);
        }

        // POST: Order_Detail/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Quantity,ProductId,OrderId")] Order_Detail order_Detail)
        {
            if (ModelState.IsValid)
            {
                helper.ChangeItem<Order_Detail>(basePath + order_Detail.Id, order_Detail);
                return RedirectToAction("Index");
            }
            ViewBag.OrderId = new SelectList(await helper.GetListOfItems<Order>("api/orders/"), "Id", "Id");
            ViewBag.ProductId = new SelectList(await helper.GetListOfItems<Product>("api/products/"), "Id", "Name");
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
            helper.DeleteItem(basePath, id);

            return RedirectToAction("Index");
        }

    }
}
