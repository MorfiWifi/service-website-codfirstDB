﻿using System;
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
        public async Task<ActionResult> Create([Bind(Include = "Id,Content")] Contract contract, bool? stayOnCreatePage)
        {
            bool castedStayOnCreatePage = stayOnCreatePage.HasValue ? stayOnCreatePage.Value : false;

            if (ModelState.IsValid)
            {
                helper.CreateItem<Contract>(basePath, contract);
                notificationHelper.SuccessfulInsert(contract.Id.ToString());
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
                helper.ChangeItem<Contract>(basePath + contract.Id, contract);
                notificationHelper.SuccessfulChange(contract.Id.ToString());
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
            notificationHelper.SuccessfulDelete((await helper.GetItem<Contract>(basePath + id)).Id.ToString());
            helper.DeleteItem(basePath, id);
            return RedirectToAction("Index");
        }

    }
}
