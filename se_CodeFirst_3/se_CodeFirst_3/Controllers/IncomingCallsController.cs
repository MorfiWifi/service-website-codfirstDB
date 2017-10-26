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

namespace se_CodeFirst_3.Controllers
{
    
    public class IncomingCallsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        ConnectToWebApiHelper helper = new ConnectToWebApiHelper();

        // GET: IncomingCalls

        public async Task<ActionResult> Index()
        {
            List<IncomingCall> incomingCalls = new List<IncomingCall>();

            incomingCalls = await helper.GetListOfItems<IncomingCall>("api/incomingcalls");

            return View(incomingCalls);
        }

        // GET: IncomingCalls/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var incomingCall = await helper.GetItem<IncomingCall>("api/incomingcalls/" + id);

            if (incomingCall == null)
            {
                return HttpNotFound();
            }
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
        public ActionResult Create([Bind(Include = "Id,Message,Date")] IncomingCall incomingCall)
        {
            if (ModelState.IsValid)
            {
                helper.CreateItem<IncomingCall>("api/incomingcalls/", incomingCall);
                return RedirectToAction("Index");
            }

            return View(incomingCall);
        }

        // GET: IncomingCalls/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IncomingCall incomingCall = await helper.GetItem<IncomingCall>("api/incomingcalls/"+ id);
            if (incomingCall == null)
            {
                return HttpNotFound();
            }
            return View(incomingCall);
        }

        // POST: IncomingCalls/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Message,Date")] IncomingCall incomingCall)
        {
            if (ModelState.IsValid)
            {
                var returningIncomingCall = helper.ChangeItem<IncomingCall>("api/incomingcalls/" + incomingCall.Id, incomingCall);
                return RedirectToAction("Index");
            }
            return View(incomingCall);
        }

        // GET: IncomingCalls/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IncomingCall incomingCall = await helper.GetItem<IncomingCall>("api/incomingcalls/" + id);
            if (incomingCall == null)
            {
                return HttpNotFound();
            }
            return View(incomingCall);
        }

        // POST: IncomingCalls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            helper.DeleteItem("api/incomingcalls/", id);
            return RedirectToAction("Index");
        }

    }
}
