using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using se_CodeFirst_3.Models;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http;
using se_CodeFirst_3.Helper;
using System.Globalization;
using se_CodeFirst_3.Filters;

namespace se_CodeFirst_3.Controllers
{
#if DEBUG

#else
    [RedirectIfNotAuthorized]
#endif
    public class IncomingCallsController : Controller
    {
        ConnectToWebApiHelper helper = new ConnectToWebApiHelper();
        NotificationProviderHelper notificationHelper;
        UsefulMethodsHelper methodHelper;

        string basePath = "api/incomingCalls/";
        public IncomingCallsController()
        {
            basePath = "api/incomingCalls/";
            notificationHelper = new NotificationProviderHelper(this);
            methodHelper = new UsefulMethodsHelper();
        }

        // GET: IncomingCalls
        public async Task<ActionResult> Index()
        {
            List<IncomingCall> incomingCalls = new List<IncomingCall>();

            incomingCalls = await helper.GetListOfItems<IncomingCall>(basePath);

            //using PersianDates::
            foreach (var item in incomingCalls)
            {
                item.Date = methodHelper.ConvertDateTimeToPersian(item.Date);
            }

            return View(incomingCalls);
        }

        // GET: IncomingCalls/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var incomingCall = await helper.GetItem<IncomingCall>(basePath + id);

            if (incomingCall == null)
            {
                return HttpNotFound();
            }

            //using PersianDates::
            incomingCall.Date = methodHelper.ConvertDateTimeToPersian(incomingCall.Date);

            return View(incomingCall);
        }

        // GET: IncomingCalls/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: IncomingCalls/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Message,Date")] IncomingCall incomingCall, bool? stayOnCreatePage)
        {
            bool castedStayOnCreatePage = stayOnCreatePage.HasValue ? stayOnCreatePage.Value : false;

            var convertedDateTime = methodHelper.ConvertDateTimeToGregorian(incomingCall.Date);

            incomingCall.Date = convertedDateTime;

            if (ModelState.IsValid)
            {
                helper.CreateItem<IncomingCall>(basePath, incomingCall);
                notificationHelper.SuccessfulInsert(incomingCall.Message);
                if (castedStayOnCreatePage == true)
                {
                    return RedirectToAction("Create");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }

            notificationHelper.FailureInsert(incomingCall.Message);
            return View(incomingCall);
        }

        // GET: IncomingCalls/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IncomingCall incomingCall = await helper.GetItem<IncomingCall>(basePath + id);
            if (incomingCall == null)
            {
                return HttpNotFound();
            }

            incomingCall.Date = methodHelper.ConvertDateTimeToPersian(incomingCall.Date);

            return View(incomingCall);
        }

        // POST: IncomingCalls/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Message,Date")] IncomingCall incomingCall)
        {
            var convertedDateTime = methodHelper.ConvertDateTimeToGregorian(incomingCall.Date);

            incomingCall.Date = convertedDateTime;

            if (ModelState.IsValid)
            {
                var returningIncomingCall = helper.ChangeItem<IncomingCall>(basePath + incomingCall.Id, incomingCall);
                notificationHelper.SuccessfulChange(incomingCall.Message);
                return RedirectToAction("Index");
            }

            notificationHelper.FailureChange(incomingCall.Message);
            return View(incomingCall);
        }

        // GET: IncomingCalls/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IncomingCall incomingCall = await helper.GetItem<IncomingCall>(basePath + id);
            if (incomingCall == null)
            {
                return HttpNotFound();
            }

            //using PersianDates::
            incomingCall.Date = methodHelper.ConvertDateTimeToPersian(incomingCall.Date);

            return View(incomingCall);
        }

        // POST: IncomingCalls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            notificationHelper.SuccessfulDelete((await helper.GetItem<IncomingCall>(basePath + id)).Message);
            helper.DeleteItem(basePath, id);
            return RedirectToAction("Index");
        }

    }
}
