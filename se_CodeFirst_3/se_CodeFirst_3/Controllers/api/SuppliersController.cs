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
    public class SuppliersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Suppliers
        public IQueryable<Supplier> GetSuppliers()
        {
            return from item in db.Suppliers
                   where item.IsDeleted == false
                   select item;
        }

        [Route("api/AllSuppliers")]
        public IQueryable<Supplier> GetAllSuppliers()
        {
            return db.Suppliers;
        }

        // GET: api/Suppliers/5
        [ResponseType(typeof(SupplierInformationViewModel))]
        public IHttpActionResult GetSupplier(int id)
        {
            var supplier = db.Suppliers.Find(id);

            if (supplier == null)
            {
                return NotFound();
            }

            var unitsInStock = (from item in supplier.Products
                                select item.UnitsInStock).Sum();

            var price = (from item in supplier.Products
                         select item.UnitsInStock * item.UnitPrice).Sum();

            SupplierInformationViewModel supplierInformation = new SupplierInformationViewModel
            {
                Address = supplier.Address,
                Name = supplier.Name,
                CompanyName = supplier.CompanyName,
                PhoneNumber = supplier.PhoneNumber,
                Products = supplier.Products,
                UnitsInStock = unitsInStock,
                Price = price
            };

            return Ok(supplierInformation);
        }

        // PUT: api/Suppliers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSupplier(int id, Supplier supplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != supplier.Id)
            {
                return BadRequest();
            }

            db.Entry(supplier).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplierExists(id))
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

        // POST: api/Suppliers
        [ResponseType(typeof(Supplier))]
        public IHttpActionResult PostSupplier(Supplier supplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Suppliers.Add(supplier);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = supplier.Id }, supplier);
        }

        // DELETE: api/Suppliers/5
        [ResponseType(typeof(Supplier))]
        public IHttpActionResult DeleteSupplier(int id)
        {
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return NotFound();
            }

            db.Suppliers.Remove(supplier);
            db.SaveChanges();

            return Ok(supplier);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SupplierExists(int id)
        {
            return db.Suppliers.Count(e => e.Id == id) > 0;
        }
    }
}