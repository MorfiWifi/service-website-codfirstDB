using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using se_CodeFirst_3.Models;

namespace se_CodeFirst_3.Controllers.api
{
#if DEBUG

#else
    [Authorize(Roles = "Administrator,Secretary")]
#endif
    public class IncomingCallsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/IncomingCalls
        public IQueryable<IncomingCall> GetIncomingCalls()
        {
            return db.IncomingCalls;
        }

        // GET: api/IncomingCalls/5
        [ResponseType(typeof(IncomingCall))]
        public IHttpActionResult GetIncomingCall(int id)
        {
            IncomingCall incomingCall = db.IncomingCalls.Find(id);
            if (incomingCall == null)
            {
                return NotFound();
            }

            return Ok(incomingCall);
        }

        // PUT: api/IncomingCalls/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutIncomingCall(int id, IncomingCall incomingCall)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != incomingCall.Id)
            {
                return BadRequest();
            }

            db.Entry(incomingCall).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IncomingCallExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/IncomingCalls
        [ResponseType(typeof(IncomingCall))]
        public IHttpActionResult PostIncomingCall(IncomingCall incomingCall)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.IncomingCalls.Add(incomingCall);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = incomingCall.Id }, incomingCall);
        }

        // DELETE: api/IncomingCalls/5
        [ResponseType(typeof(IncomingCall))]
        public IHttpActionResult DeleteIncomingCall(int id)
        {
            IncomingCall incomingCall = db.IncomingCalls.Find(id);
            if (incomingCall == null)
            {
                return NotFound();
            }

            db.IncomingCalls.Remove(incomingCall);
            db.SaveChanges();

            return Ok(incomingCall);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool IncomingCallExists(int id)
        {
            return db.IncomingCalls.Count(e => e.Id == id) > 0;
        }
    }
}