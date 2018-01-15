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
using se_CodeFirst_3.Helper;
using se_CodeFirst_3.Filters;

namespace se_CodeFirst_3.Controllers.api
{
#if DEBUG

#else
    [Authorize]
    [LogApi]
#endif

    public class IncomingCallsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/IncomingCalls
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "IncomingCall", ClaimValue = "Get")]
#endif

        public IQueryable<IncomingCall> GetIncomingCalls([FromUri]IncomingCallDTO incomingCallDTO)
        {
            UsefulMethodsHelper methodHelper = new UsefulMethodsHelper();
            IQueryable<IncomingCall> incomingCalls = db.IncomingCalls;
            incomingCalls = from item in incomingCalls
                     where
                        (!String.IsNullOrEmpty(incomingCallDTO.Message) ? item.Message.Contains(incomingCallDTO.Message) : true) &&
                        ((incomingCallDTO.MinDate.Year != 1) ?
                            ((item.Date.Year > incomingCallDTO.MinDate.Year) || 
                            (item.Date.Year == incomingCallDTO.MinDate.Year && item.Date.Month > incomingCallDTO.MinDate.Month) ||
                            (item.Date.Year == incomingCallDTO.MinDate.Year && item.Date.Month == incomingCallDTO.MinDate.Month && item.Date.Day > incomingCallDTO.MinDate.Day))
                            : true)
                        //     &&
                        //((incomingCallDTO.MaxDate.Year != 1) ?
                        //    ((item.Date.Year < incomingCallDTO.MaxDate.Year) ||
                        //    (item.Date.Year == incomingCallDTO.MaxDate.Year && item.Date.Month < incomingCallDTO.MaxDate.Month) ||
                        //    (item.Date.Year == incomingCallDTO.MaxDate.Year && item.Date.Month == incomingCallDTO.MaxDate.Month && item.Date.Day < incomingCallDTO.MaxDate.Day))
                        //    : true)
                            select item;

            return incomingCalls;
        }

        // GET: api/IncomingCalls/5
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "IncomingCall", ClaimValue = "Get")]
#endif
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
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "IncomingCall", ClaimValue = "Put")]
#endif
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
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "IncomingCall", ClaimValue = "Post")]
#endif
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
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "IncomingCall", ClaimValue = "Delete")]
#endif
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