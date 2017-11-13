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

namespace se_CodeFirst_3.Controllers.api
{
#if DEBUG

#else
    [Authorize(Roles = "Administrator")]
#endif
    public class RolesController : ApiController
    {
        private ApplicationDbContext db;
        private RoleStore<IdentityRole> roleStore;
        private RoleManager<IdentityRole> roleManager;

        public RolesController()
        {
            db = new ApplicationDbContext();
            roleStore = new RoleStore<IdentityRole>(db);
            roleManager = new RoleManager<IdentityRole>(roleStore);
        }


        // GET: api/Roles
        public IQueryable<IdentityRole> GetRoleViewModels()
        {
            return roleManager.Roles;
        }

        // GET: api/Roles/5
        [ResponseType(typeof(IdentityRole))]
        public async Task<IHttpActionResult> GetRoleViewModel(string id)
        {
            IdentityRole identityRole = roleManager.FindById(id);
            if (identityRole == null)
            {
                return NotFound();
            }

            return Ok(identityRole);
        }

        // PUT: api/Roles/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutRoleViewModel(string id, IdentityRole identityRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != identityRole.Id)
            {
                return BadRequest();
            }

            roleManager.Update(identityRole);

            return StatusCode(HttpStatusCode.NoContent);
        }

        //POST: api/Roles
        [ResponseType(typeof(IdentityRole))]
        public async Task<IHttpActionResult> PostRoleViewModel(IdentityRole identityRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            roleManager.Create(identityRole);

            return CreatedAtRoute("DefaultApi", new { id = identityRole.Id }, identityRole);
        }

        // DELETE: api/Roles/5
        [ResponseType(typeof(IdentityRole))]
        public async Task<IHttpActionResult> DeleteRoleViewModel(string id)
        {
            IdentityRole identityRole = roleManager.FindById(id);
            if (identityRole == null)
            {
                return NotFound();
            }

            roleManager.Delete(identityRole);

            return Ok(identityRole);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RoleViewModelExists(string id)
        {
            return roleManager.Roles.Count(e => e.Id == id) > 0;
        }
    }
}