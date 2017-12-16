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
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web.Security;
using se_CodeFirst_3.Filters;
using se_CodeFirst_3.Helper;

namespace se_CodeFirst_3.Controllers.api
{

#if DEBUG

#else
    [Authorize]//[Authorize(Roles = "Administrator")]
#endif
    [LogApi]
    public class UsersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UsefulMethodsHelper methodsHelper = new UsefulMethodsHelper();

        // GET: api/Users
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "User", ClaimValue = "Get")]
#endif
        public IQueryable<ApplicationUser> GetApplicationUsers([FromUri]UserDTO userDTO)
        {
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);

            //IQueryable<ApplicationUser> usersToReturn = db.Users;
            IQueryable<ApplicationUser> users = db.Users;
            //foreach (var item in users)
            //{
            //    if (!userManager.IsInRole(item.Id, "Administrator"))
            //    {
            //        usersToReturn.Add(item);
            //    }
            //}

            users = from item in users
                    where
                       (!String.IsNullOrEmpty(userDTO.Email) ? item.Email.Contains(userDTO.Email) : true) &&
                       (!String.IsNullOrEmpty(userDTO.UserName) ? item.UserName.Contains(userDTO.UserName) : true) &&
                       (userDTO.MinSalary != 0 ? item.Salary > userDTO.MinSalary : true) &&
                       (userDTO.MinOvertime != 0 ? item.OverTime > userDTO.MinOvertime : true) &&
                       (userDTO.MinBenefits != 0 ? item.Benefits > userDTO.MinBenefits : true) &&
                       (userDTO.MinAbsentDays != 0 ? item.AbsentDays > userDTO.MinAbsentDays : true) &&
                       (userDTO.MaxSalary != 0 ? item.Salary < userDTO.MaxSalary : true) &&
                       (userDTO.MaxOvertime != 0 ? item.OverTime < userDTO.MaxOvertime : true) &&
                       (userDTO.MaxBenefits != 0 ? item.Benefits < userDTO.MaxBenefits : true) &&
                       (userDTO.MaxAbsentDays != 0 ? item.AbsentDays < userDTO.MaxAbsentDays : true)
                    select item;

            return users.AsQueryable();
        }

        // GET: api/Users/5
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "User", ClaimValue = "Get")]
#endif
        [ResponseType(typeof(ApplicationUser))]
        public IHttpActionResult GetApplicationUser(string id)
        {
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);

            var finalSalary = applicationUser.Salary +
                applicationUser.OverTime * 40 +
                applicationUser.Benefits -
                applicationUser.AbsentDays * 10;

            var roles = userManager.GetRoles(applicationUser.Id);

            UserViewModel userInformation = new UserViewModel
            {
                UserName = applicationUser.UserName,
                Email = applicationUser.Email,
                Salary = applicationUser.Salary,
                Benefits = applicationUser.Benefits,
                AbsentDays = applicationUser.AbsentDays,
                OverTime = applicationUser.OverTime,
                FinalSalary = finalSalary,
                Roles = roles
            };


            return Ok(userInformation);
        }

#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "User", ClaimValue = "Get")]
#endif
        [ResponseType(typeof(ApplicationUser))]
        [Route("api/UsersA/{id}")]
        public IHttpActionResult GetApplicationUserA(string id)
        {
            return Ok(db.Users.Find(id));
        }

        // PUT: api/Users/5
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "User", ClaimValue = "Put")]
#endif
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutApplicationUser(string id, RegisterBindingModel model)
        {
            if (!ModelState.IsValid || methodsHelper.IsUserInRole(db, id))
            {
                return BadRequest(ModelState);
            }

            if (id != model.Id)
            {
                return BadRequest();
            }

            //var user = new ApplicationUser()
            //{
            //    UserName = registerBindingModel.Email,
            //    Email = registerBindingModel.Email,
            //    Benefits = registerBindingModel.Benefits,
            //    Salary = registerBindingModel.Salary
            //};
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);

            var user = userManager.FindById(id);

            db.Users.Attach(user);
            var entry = db.Entry(user);
            entry.Property(e2 => e2.Email).CurrentValue = model.Email;
            entry.Property(e2 => e2.Email).IsModified = true;
            entry.Property(e2 => e2.UserName).CurrentValue = model.Email;
            entry.Property(e2 => e2.UserName).IsModified = true;
            entry.Property(e2 => e2.AbsentDays).CurrentValue = model.AbsentDays;
            entry.Property(e2 => e2.AbsentDays).IsModified = true;
            entry.Property(e2 => e2.Benefits).CurrentValue = model.Benefits;
            entry.Property(e2 => e2.Benefits).IsModified = true;
            entry.Property(e2 => e2.OverTime).CurrentValue = model.OverTime;
            entry.Property(e2 => e2.OverTime).IsModified = true;
            entry.Property(e2 => e2.Salary).CurrentValue = model.Salary;
            entry.Property(e2 => e2.Salary).IsModified = true;

            //next: role and password



            //MembershipUser mu = Membership.GetUser(user.UserName);
            //var result = mu.ChangePassword(mu.ResetPassword(), model.Password);

            userManager.RemoveFromRole(user.Id, user.Roles.FirstOrDefault().ToString());
            userManager.AddToRole(user.Id, model.Role);

            db.SaveChanges();
            //IdentityResult result = await userManager.ChangePasswordAsync(user.Id, model.Password);

            //userManager.RemoveFromRole(user.Id, user.Roles.FirstOrDefault().);

            //if (!result)
            //{
            //    return Conflict();
            //}

            return StatusCode(HttpStatusCode.NoContent);



            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //if (id != applicationUser.Id)
            //{
            //    return BadRequest();
            //}

            //db.Entry(applicationUser).State = EntityState.Modified;

            //try
            //{
            //    db.SaveChanges();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!ApplicationUserExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Users
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "User", ClaimValue = "Post")]
#endif
        [ResponseType(typeof(ApplicationUser))]
        public IHttpActionResult PostApplicationUser(ApplicationUser applicationUser)
        {
            if (!ModelState.IsValid || methodsHelper.IsUserInRole(db, applicationUser.Id))
            {
                return BadRequest(ModelState);
            }

            db.Users.Add(applicationUser);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ApplicationUserExists(applicationUser.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = applicationUser.Id }, applicationUser);
        }

        // DELETE: api/Users/5
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "User", ClaimValue = "Delete")]
#endif
        [ResponseType(typeof(ApplicationUser))]
        public async Task<IHttpActionResult> DeleteApplicationUser(string id)
        {
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null || methodsHelper.IsUserInRole(db, id))
            {
                return NotFound();
            }

            db.Users.Remove(applicationUser);

            //var userStore = new UserStore<ApplicationUser>(db);
            //var userManager = new UserManager<ApplicationUser>(userStore);

            //await userManager.DeleteAsync(applicationUser);

            await db.SaveChangesAsync();

            return Ok(applicationUser);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ApplicationUserExists(string id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }
    }
}