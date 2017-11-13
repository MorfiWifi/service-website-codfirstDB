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
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Security.Claims;

namespace se_CodeFirst_3.Controllers.api
{
#if DEBUG

#else
    [Authorize(Roles = "Administrator")]
#endif
    public class ClaimsController : ApiController
    {
        private ApplicationDbContext db;
        private RoleStore<IdentityRole> roleStore;
        private RoleManager<IdentityRole> roleManager;
        UserStore<ApplicationUser> userStore;
        UserManager<ApplicationUser> userManager;

        public ClaimsController()
        {
            db = new ApplicationDbContext();
            roleStore = new RoleStore<IdentityRole>(db);
            roleManager = new RoleManager<IdentityRole>(roleStore);
            userStore = new UserStore<ApplicationUser>(db);
            userManager = new UserManager<ApplicationUser>(userStore);
        }

        // GET: api/Claims
        public IQueryable<ClaimViewModel> GetClaims()
        {
            return db.Claims;
        }

        [Route("api/ClaimsA")]
        public IQueryable<ClaimViewModel> GetClaimsA()
        {
            return db.Claims;
        }

        // get list of claims by user id
        [Route("api/ClaimsA/{id}")]
        public ICollection<IdentityUserClaim> GetClaimsA(string id)
        {
            var user = db.Users.Find(id);
            if (id == null)
            {
                return null;
            }

            return user.Claims;
        }

        // GET: api/Claims/5
        [ResponseType(typeof(ClaimViewModel))]
        public async Task<IHttpActionResult> GetClaimViewModel(int id)
        {
            ClaimViewModel claimViewModel = await db.Claims.FindAsync(id);
            if (claimViewModel == null)
            {
                return NotFound();
            }

            return Ok(claimViewModel);
        }

        // PUT: api/Claims/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutClaimViewModel(int id, ClaimViewModel claimViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != claimViewModel.Id)
            {
                return BadRequest();
            }

            db.Entry(claimViewModel).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClaimViewModelExists(id))
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

        // POST: api/Claims
        [ResponseType(typeof(ClaimViewModel))]
        [Route("api/Claims/{id}/{claimId}")]
        public async Task<IHttpActionResult> PostClaimViewModel(string id, int claimId)
        {
            var userClaims = userManager.GetClaims(id);
            var claimViewModel = db.Claims.Find(claimId);
            var claim = new Claim(claimViewModel.Type, claimViewModel.Value);

            var claimAlreadyExists = (from item in userClaims
                                      where item.Type == claim.Type && item.Value == claim.Value
                                      select item).Count();

            if (claimAlreadyExists > 0)
            {
                //remove claim:
                userManager.RemoveClaim(id, claim);
                return Ok(claim);
            }
            else
            {
                //add claim:
                userManager.AddClaim(id, claim);
                return Ok(claim);
            }

        }

        // DELETE: api/Claims/5
        [ResponseType(typeof(ClaimViewModel))]
        public async Task<IHttpActionResult> DeleteClaimViewModel(int id)
        {
            ClaimViewModel claimViewModel = await db.Claims.FindAsync(id);
            if (claimViewModel == null)
            {
                return NotFound();
            }

            db.Claims.Remove(claimViewModel);
            await db.SaveChangesAsync();

            return Ok(claimViewModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClaimViewModelExists(int id)
        {
            return db.Claims.Count(e => e.Id == id) > 0;
        }
    }
}