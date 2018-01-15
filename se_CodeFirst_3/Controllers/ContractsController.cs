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
    public class ContractsController : Controller
    {
        ConnectToWebApiHelper helper = new ConnectToWebApiHelper();
        NotificationProviderHelper notificationHelper;

        string basePath = "api/contracts/";
        public ContractsController()
        {
            basePath = "api/contracts/";
            notificationHelper = new NotificationProviderHelper(this);
        }

        // GET: Contracts
        [ClaimsAuthorization(ClaimType = "Contract", ClaimValue = "Get")]
        public async Task<ActionResult> Index(int? page, bool? searching, Contract contract)
        {
            List<Contract> contracts = await helper.GetListOfItems<Contract>(basePath);
            bool castedSearching = searching.HasValue ? searching.Value : false;

            if (contract != null)
            {
                contracts = await helper.GetListOfItems<Contract>("api/contracts",
                    "?Content=" + contract.Content
                    );
            }
            else
            {
                contracts = await helper.GetListOfItems<Contract>(basePath);
            }

            if (castedSearching)
            {
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return Json(contracts.ToPagedList(pageNumber, pageSize), JsonRequestBehavior.AllowGet);
            }
            else
            {
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(contracts.ToPagedList(pageNumber, pageSize));
            }
        }

        // GET: Contracts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contract contract = await helper.GetItem<Contract>(basePath + id);
            if (contract == null)
            {
                return HttpNotFound();
            }
            return View(contract);
        }

        // GET: Contracts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contracts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Content")] Contract contract, bool? stayOnCreatePage)
        {
            bool castedStayOnCreatePage = stayOnCreatePage.HasValue ? stayOnCreatePage.Value : false;

            if (ModelState.IsValid)
            {
                var itemCreated = helper.CreateItem<Contract>(basePath, contract);
                if (itemCreated != null)
                {
                    notificationHelper.SuccessfulInsert(contract.Id.ToString());
                }
                else
                {
                    notificationHelper.FailureInsert(contract.Id.ToString());
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

            notificationHelper.FailureInsert(contract.Id.ToString());
            return View(contract);
        }

        // GET: Contracts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contract contract = await helper.GetItem<Contract>(basePath + id);
            if (contract == null)
            {
                return HttpNotFound();
            }
            return View(contract);
        }

        // POST: Contracts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Content")] Contract contract)
        {
            if (ModelState.IsValid)
            {
                var itemEdited = helper.ChangeItem<Contract>(basePath + contract.Id, contract);
                if (itemEdited != null)
                    notificationHelper.SuccessfulChange(contract.Id.ToString());
                else
                    notificationHelper.FailureChange(contract.Id.ToString());

                return RedirectToAction("Index");
            }

            notificationHelper.FailureChange(contract.Id.ToString());
            return View(contract);
        }

        // GET: Contracts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contract contract = await helper.GetItem<Contract>(basePath + id);
            if (contract == null)
            {
                return HttpNotFound();
            }
            return View(contract);
        }

        // POST: Contracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            string deletedItem = (await helper.GetItem<Contract>(basePath + id)).Id.ToString();
            bool successfulDelete = helper.DeleteItem(basePath, id);
            if (successfulDelete)
            {
                notificationHelper.SuccessfulDelete(deletedItem);
            }
            else
            {
                notificationHelper.CustomFailureMessage("خطا در حذف " + deletedItem + ". این متن قرارداد در جایی استفاده شده است.");
            }
            return RedirectToAction("Index");
        }

    }
}
