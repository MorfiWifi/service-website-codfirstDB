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
using PagedList;

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
        public async Task<ActionResult> Index(int? page, bool? searching, IncomingCallDTO incomingCallDTO)
        {
            List<IncomingCall> incomingCalls = new List<IncomingCall>();
            incomingCalls = await helper.GetListOfItems<IncomingCall>(basePath);
            bool castedSearching = searching.HasValue ? searching.Value : false;

            if (incomingCallDTO != null)
            {
                incomingCalls = await helper.GetListOfItems<IncomingCall>("api/incomingCalls",
                    "?Message=" + incomingCallDTO.Message + "&" +
                    "MinDate=" + methodHelper.ConvertDateTimeToGregorian(incomingCallDTO.MinDate) + "&" +
                    "MaxDate=" + methodHelper.ConvertDateTimeToGregorian(incomingCallDTO.MaxDate)
                    );
            }
            else
            {
                incomingCalls = await helper.GetListOfItems<IncomingCall>(basePath);
            }

            //using PersianDates::
            foreach (var item in incomingCalls)
            {
                item.Date = methodHelper.ConvertDateTimeToPersian(item.Date);
            }

            if (castedSearching)
            {
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return Json(incomingCalls.ToPagedList(pageNumber, pageSize), JsonRequestBehavior.AllowGet);
            }
            else
            {
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(incomingCalls.ToPagedList(pageNumber, pageSize));
            }
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
            ViewBag.IncomingCallDateTime = methodHelper.ConvertDateTimeToPersian(incomingCall.Date, "yyyy/MM/dd HH:mm:ss");

            return View(incomingCall);
        }

        // GET: IncomingCalls/Create
        public ActionResult Create()
        {
            ViewBag.DateTimeNow = methodHelper.ConvertDateTimeToPersian(DateTime.Now, "yyyy/MM/dd HH:mm:ss");
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
                var itemCreated = helper.CreateItem<IncomingCall>(basePath, incomingCall);
                if (itemCreated != null)
                {
                    notificationHelper.SuccessfulInsert(incomingCall.Message);
                }
                else
                {
                    notificationHelper.FailureInsert(incomingCall.Message);
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

            ViewBag.IncomingCallDateTime = methodHelper.ConvertDateTimeToPersian(incomingCall.Date, "yyyy/MM/dd HH:mm:ss");

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
                var itemEdited = helper.ChangeItem<IncomingCall>(basePath + incomingCall.Id, incomingCall);
                if (itemEdited != null)
                    notificationHelper.SuccessfulChange(incomingCall.Message);
                else
                    notificationHelper.FailureChange(incomingCall.Message);

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
            ViewBag.IncomingCallDateTime = methodHelper.ConvertDateTimeToPersian(incomingCall.Date, "yyyy/MM/dd HH:mm:ss");

            return View(incomingCall);
        }

        // POST: IncomingCalls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            string deletedItem = (await helper.GetItem<IncomingCall>(basePath + id)).Message;
            bool successfulDelete = helper.DeleteItem(basePath, id);
            if (successfulDelete)
                notificationHelper.SuccessfulDelete(deletedItem);
            else
                notificationHelper.CustomFailureMessage("خطا در حذف " + deletedItem);

            return RedirectToAction("Index");

            
        }

    }
}
