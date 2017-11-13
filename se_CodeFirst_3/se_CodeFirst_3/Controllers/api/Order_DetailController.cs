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
    [Authorize]//[Authorize(Roles = "Administrator,Secretary")]
#endif
    public class Order_DetailController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Order_Detail
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "Order_Detail", ClaimValue = "Get")]
#endif
        public IQueryable<Order_Detail> GetOrder_Details()
        {
            return db.Order_Details;
        }

        // GET: api/Order_Detail/5
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "Order_Detail", ClaimValue = "Get")]
#endif
        [ResponseType(typeof(Order_Detail))]
        public IHttpActionResult GetOrder_Detail(int id)
        {
            Order_Detail order_Detail = db.Order_Details.Find(id);
            if (order_Detail == null)
            {
                return NotFound();
            }

            return Ok(order_Detail);
        }

        // PUT: api/Order_Detail/5
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "Order_Detail", ClaimValue = "Put")]
#endif
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrder_Detail(int id, Order_Detail order_Detail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order_Detail.Id)
            {
                return BadRequest();
            }

            //tip: order_detail is for after changing and order_detail with Id=id is the one before editing
            var quantityBefore = db.Order_Details.Find(id).Quantity;

            db.Entry(order_Detail).State = EntityState.Modified;

            try
            {
                db.SaveChanges();

                //now we have to update products table too:
                var product = (from item in db.Products
                               where item.Id == order_Detail.ProductId
                               select item).SingleOrDefault();

                product.UnitsInStock = product.UnitsInStock + quantityBefore;
                product.UnitsInStock = product.UnitsInStock - order_Detail.Quantity;

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Order_DetailExists(id))
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

        // POST: api/Order_Detail
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "Order_Detail", ClaimValue = "Post")]
#endif
        [ResponseType(typeof(Order_Detail))]
        public IHttpActionResult PostOrder_Detail(Order_Detail order_Detail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = (from item in db.Products
                           where item.Id == order_Detail.ProductId
                           select item).SingleOrDefault();

            if (product.UnitsInStock >= order_Detail.Quantity)
            {
                db.Order_Details.Add(order_Detail);

                //now we have to update products table too:
                product.UnitsInStock = product.UnitsInStock - order_Detail.Quantity;
                db.Entry(product).State = EntityState.Modified;

                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = order_Detail.Id }, order_Detail);
            }
            else
            {
                return BadRequest(ModelState + "تعداد کالاها نمی تواند از موجودی بیشتر باشد");
            }

        }

        // DELETE: api/Order_Detail/5
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "Order_Detail", ClaimValue = "Delete")]
#endif
        [ResponseType(typeof(Order_Detail))]
        public IHttpActionResult DeleteOrder_Detail(int id)
        {
            Order_Detail order_Detail = db.Order_Details.Find(id);
            if (order_Detail == null)
            {
                return NotFound();
            }

            db.Order_Details.Remove(order_Detail);

            //now we have to update products table too:
            var product = (from item in db.Products
                           where item.Id == order_Detail.ProductId
                           select item).SingleOrDefault();

            product.UnitsInStock = product.UnitsInStock + order_Detail.Quantity;
            db.Entry(product).State = EntityState.Modified;

            db.SaveChanges();

            return Ok(order_Detail);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Order_DetailExists(int id)
        {
            return db.Order_Details.Count(e => e.Id == id) > 0;
        }
    }
}