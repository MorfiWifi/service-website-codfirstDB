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
    public class CustomersController : Controller
    {
        ConnectToWebApiHelper helper = new ConnectToWebApiHelper();
        NotificationProviderHelper notificationHelper;

        string basePath = "api/customers/";
        public CustomersController()
        {
            basePath = "api/customers/";
            notificationHelper = new NotificationProviderHelper(this);
        }

        // GET: Customers
        public async Task<ActionResult> Index(bool? includeDeletedItems, int? page, bool? searching, Customer customer)
        {
            bool castedIncludeDeletedItems = includeDeletedItems.HasValue ? includeDeletedItems.Value : false;
            bool castedSearching = searching.HasValue ? searching.Value : false;

            List<Customer> customers = new List<Customer>();

            if (customer != null)
            {
                customers = await helper.GetListOfItems<Customer>("api/customers",
                    "?CompanyName=" + customer.CompanyName + "&" +
                    "Name=" + customer.Name + "&" +
                    "PhoneNumber=" + customer.PhoneNumber
                    );
            }
            else if (castedIncludeDeletedItems == false)
            {
                customers = await helper.GetListOfItems<Customer>(basePath);
            }
            else
            {
                customers = await helper.GetListOfItems<Customer>("api/allSuppliers");
            }

            if (castedSearching)
            {
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return Json(customers.ToPagedList(pageNumber, pageSize), JsonRequestBehavior.AllowGet);
            }
            else
            {
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(customers.ToPagedList(pageNumber, pageSize));
            }
            
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
        public ActionResult Create([Bind(Include = "Id,Name,CompanyName,PhoneNumber")] Customer customer, bool? stayOnCreatePage)
        {
            bool castedStayOnCreatePage = stayOnCreatePage.HasValue ? stayOnCreatePage.Value : false;

            if (ModelState.IsValid)
            {
                helper.CreateItem<Customer>(basePath, customer);
                notificationHelper.SuccessfulInsert(customer.Name);
                if (castedStayOnCreatePage == true)
                {
                    return RedirectToAction("Create");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }

            notificationHelper.FailureInsert(customer.Name);
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
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,CompanyName,PhoneNumber,IsDeleted")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                helper.ChangeItem<Customer>(basePath + customer.Id, customer);
                notificationHelper.SuccessfulChange(customer.Name);
                return RedirectToAction("Index");
            }

            notificationHelper.FailureChange(customer.Name);
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
            notificationHelper.SuccessfulDelete((await helper.GetItem<Customer>(basePath + id)).Name);
            helper.DeleteItem(basePath, id);
            return RedirectToAction("Index");
        }

    }
}
