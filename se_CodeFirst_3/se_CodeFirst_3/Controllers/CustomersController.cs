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
    public class CustomersController : Controller
    {
        ConnectToWebApiHelper helper = new ConnectToWebApiHelper();

        string basePath = "api/customers/";
        public CustomersController()
        {
            basePath = "api/customers/";
        }

        // GET: Customers
        public async Task<ActionResult> Index()
        {
            List<Customer> customers = await helper.GetListOfItems<Customer>(basePath);

            return View(customers);
        }

        // GET: Customers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerViewModel customerInformation = await helper.GetItem<CustomerViewModel>(basePath + id);
            if (customerInformation == null)
            {
                return HttpNotFound();
            }
            return View(customerInformation);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,CompanyName,PhoneNumber")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                helper.CreateItem<Customer>(basePath, customer);
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = await helper.GetItem<Customer>(basePath + id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,CompanyName,PhoneNumber")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                helper.ChangeItem<Customer>(basePath + customer.Id, customer);
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = await helper.GetItem<Customer>(basePath + id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            helper.DeleteItem(basePath, id);

            return RedirectToAction("Index");
        }

    }
}
