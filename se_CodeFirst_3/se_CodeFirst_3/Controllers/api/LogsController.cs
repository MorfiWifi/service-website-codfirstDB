using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using se_CodeFirst_3.Models;

namespace se_CodeFirst_3.Controllers.api
{
    public class LogsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Logs
        public IQueryable<Log> GetLogs()
        {
            return db.Logs;
        }

        // GET: api/Logs/5
        [ResponseType(typeof(Log))]
        public async Task<IHttpActionResult> GetLog(int id)
        {
            Log log = await db.Logs.FindAsync(id);
            if (log == null)
            {
                return NotFound();
            }

            return Ok(log);
        }

        // PUT: api/Logs/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutLog(int id, Log log)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != log.Id)
            {
                return BadRequest();
            }

            db.Entry(log).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LogExists(id))
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

        // POST: api/Logs
        [ResponseType(typeof(Log))]
        public async Task<IHttpActionResult> PostLog(Log log)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Logs.Add(log);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = log.Id }, log);
        }

        // DELETE: api/Logs/5
        [ResponseType(typeof(Log))]
        public async Task<IHttpActionResult> DeleteLog(int id)
        {
            Log log = await db.Logs.FindAsync(id);
            if (log == null)
            {
                return NotFound();
            }

            db.Logs.Remove(log);
            await db.SaveChangesAsync();

            return Ok(log);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LogExists(int id)
        {
            return db.Logs.Count(e => e.Id == id) > 0;
        }
    }
}