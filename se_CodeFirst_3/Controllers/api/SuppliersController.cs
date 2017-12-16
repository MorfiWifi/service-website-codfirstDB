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
using se_CodeFirst_3.Filters;

namespace se_CodeFirst_3.Controllers.api
{
#if DEBUG

#else
    [Authorize]//[Authorize(Roles = "Administrator,Secretary,StoreKeeper,Accountant")]
#endif
    [LogApi]
    public class SuppliersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Suppliers
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "Supplier", ClaimValue = "Get")]
#endif
        public IQueryable<Supplier> GetSuppliers([FromUri]Supplier supplier)
        {
            IQueryable<Supplier> suppliers = from item in db.Suppliers
                                             where item.IsDeleted == false
                                             select item;

            suppliers = from item in suppliers
                        where
                            (!String.IsNullOrEmpty(supplier.CompanyName) ? item.CompanyName.Contains(supplier.CompanyName) : true) &&
                            (!String.IsNullOrEmpty(supplier.Name) ? item.Name.Contains(supplier.Name) : true) &&
                            (!String.IsNullOrEmpty(supplier.Address) ? item.Name.Contains(supplier.Address) : true) &&
                            (supplier.PhoneNumber != 0 ? item.PhoneNumber.ToString().Contains(supplier.PhoneNumber.ToString()) : true)
                        select item;

            return suppliers;
        }

#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "Supplier", ClaimValue = "Get")]
#endif
        [Route("api/AllSuppliers")]
        public IQueryable<Supplier> GetAllSuppliers()
        {
            return db.Suppliers;
        }

        // GET: api/Suppliers/5
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "Supplier", ClaimValue = "Get")]
#endif
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
                         select item.UnitsInStock * item.BuyUnitPrice).Sum();

            //Get products count and price that used in orders and add it too:
            foreach (var item in db.Order_Details)
            {
                if (item.Product.SupplierId == id)
                {
                    unitsInStock += item.Quantity;
                    price += item.Quantity * item.Product.BuyUnitPrice;
                }
            }

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
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "Supplier", ClaimValue = "Put")]
#endif
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
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "Supplier", ClaimValue = "Post")]
#endif
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
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "Supplier", ClaimValue = "Delete")]
#endif
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