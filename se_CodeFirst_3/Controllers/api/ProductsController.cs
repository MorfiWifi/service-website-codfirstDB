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
    public class ProductsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Products
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "Product", ClaimValue = "Get")]
#endif
        public IQueryable<Product> GetProducts([FromUri]ProductDTO productDTO)
        {
            IQueryable<Product> products = db.Products;
            products = from item in products
                       where
                          (!String.IsNullOrEmpty(productDTO.Name) ? item.Name.Contains(productDTO.Name) : true) &&
                          (!String.IsNullOrEmpty(productDTO.SupplierCompanyName) ? item.Supplier.CompanyName.Contains(productDTO.SupplierCompanyName) : true) &&
                          (productDTO.MinUnitsInStock != 0 ? item.UnitsInStock > productDTO.MinUnitsInStock : true) &&
                          (productDTO.MinUnitPrice != 0 ? item.BuyUnitPrice > productDTO.MinUnitPrice : true) &&
                          (productDTO.MinSellUnitPrice != 0 ? item.SellUnitPrice > productDTO.MinSellUnitPrice : true) &&
                          (productDTO.MaxUnitsInStock != 0 ? item.UnitsInStock < productDTO.MaxUnitsInStock : true) &&
                          (productDTO.MaxUnitPrice != 0 ? item.BuyUnitPrice < productDTO.MaxUnitPrice : true) &&
                          (productDTO.MaxSellUnitPrice != 0 ? item.SellUnitPrice < productDTO.MaxSellUnitPrice : true)
                       select item;

            return products;
        }

        // GET: api/Products/5
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "Product", ClaimValue = "Get")]
#endif
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Products/5
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "Product", ClaimValue = "Put")]
#endif
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Products
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "Product", ClaimValue = "Post")]
#endif
        [ResponseType(typeof(Product))]
        public IHttpActionResult PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //first, we have to set the supplier for prodoct; otherwise, EF create new Supplier in DB. is that what we want?no
            //product.Supplier = db.Suppliers.Find(product.Supplier.Id);
            db.Products.Add(product);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "Product", ClaimValue = "Delete")]
#endif
        [ResponseType(typeof(Product))]
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            db.SaveChanges();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.Id == id) > 0;
        }
    }
}