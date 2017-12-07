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
    public class ContractsController : Controller
    {
        ConnectToWebApiHelper helper = new ConnectToWebApiHelper();

        string basePath = "api/contracts/";
        public ContractsController()
        {
            basePath = "api/contracts/";
        }

        // GET: Contracts
        public async Task<ActionResult> Index()
        {
            List<Contract> contracts = await helper.GetListOfItems<Contract>(basePath);

            return View(contracts);
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
        public async Task<ActionResult> Create([Bind(Include = "Id,Content")] Contract contract)
        {
            if (ModelState.IsValid)
            {
                helper.CreateItem<Contract>(basePath, contract);
                return RedirectToAction("Index");
            }

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
                helper.ChangeItem<Contract>(basePath + contract.Id, contract);
                return RedirectToAction("Index");
            }
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
            helper.DeleteItem(basePath, id);

            return RedirectToAction("Index");
        }

    }
}
