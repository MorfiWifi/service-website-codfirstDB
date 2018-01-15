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
using se_CodeFirst_3.Filters;

namespace se_CodeFirst_3.Controllers.api
{

#if DEBUG

#else
    [Authorize]//[Authorize(Roles = "Administrator,Secretary")]
    [LogApi]
#endif
    public class ContractsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Contracts
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "Contract", ClaimValue = "Get")]
#endif
        public IQueryable<Contract> GetContracts([FromUri]Contract contract)
        {
            IQueryable<Contract> contracts = db.Contracts;

            contracts = from item in contracts
                        where
                            (!String.IsNullOrEmpty(contract.Content) ? item.Content.Contains(contract.Content) : true)
                        select item;

            return contracts;
        }

        // GET: api/Contracts/5
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "Contract", ClaimValue = "Get")]
#endif
        [ResponseType(typeof(Contract))]
        public async Task<IHttpActionResult> GetContract(int id)
        {
            Contract contract = await db.Contracts.FindAsync(id);
            if (contract == null)
            {
                return NotFound();
            }

            return Ok(contract);
        }

        // PUT: api/Contracts/5
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "Contract", ClaimValue = "Put")]
#endif
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutContract(int id, Contract contract)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contract.Id)
            {
                return BadRequest();
            }

            db.Entry(contract).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContractExists(id))
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

        // POST: api/Contracts
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "Contract", ClaimValue = "Post")]
#endif
        [ResponseType(typeof(Contract))]
        public async Task<IHttpActionResult> PostContract(Contract contract)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Contracts.Add(contract);
            await db.SaveChangesAsync();
            return CreatedAtRoute("DefaultApi", new { id = contract.Id }, contract);
        }

        // DELETE: api/Contracts/5
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "Contract", ClaimValue = "Delete")]
#endif
        [ResponseType(typeof(Contract))]
        public async Task<IHttpActionResult> DeleteContract(int id)
        {
            Contract contract = await db.Contracts.FindAsync(id);
            if (contract == null)
            {
                return NotFound();
            }

            db.Contracts.Remove(contract);
            await db.SaveChangesAsync();

            return Ok(contract);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ContractExists(int id)
        {
            return db.Contracts.Count(e => e.Id == id) > 0;
        }
    }
}