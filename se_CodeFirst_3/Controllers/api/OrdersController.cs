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
using System.Data.Entity.SqlServer;

namespace se_CodeFirst_3.Controllers.api
{
#if DEBUG

#else
    [Authorize]//[Authorize(Roles = "Administrator,Secretary")]
#endif
    [LogApi]
    public class OrdersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Orders
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "Order", ClaimValue = "Get")]
#endif
        public IQueryable<Order> GetOrders([FromUri]OrderDTO orderDTO)
        {
            IQueryable<Order> orders = db.Orders;
            orders = from item in orders
                       where
                          (!String.IsNullOrEmpty(orderDTO.Content) ? item.Contract.Content.Contains(orderDTO.Content) : true) &&
                          (!String.IsNullOrEmpty(orderDTO.Customer) ? item.Customer.Name.Contains(orderDTO.Customer) : true) &&
                          ((orderDTO.MinOrderDate.Year != 1) ?
                            ((item.OrderDate.Year > orderDTO.MinOrderDate.Year) ||
                            (item.OrderDate.Year == orderDTO.MinOrderDate.Year && item.OrderDate.Month > orderDTO.MinOrderDate.Month) ||
                            (item.OrderDate.Year == orderDTO.MinOrderDate.Year && item.OrderDate.Month == orderDTO.MinOrderDate.Month && item.OrderDate.Day > orderDTO.MinOrderDate.Day))
                            : true) &&
                          ((orderDTO.MinRequiredDate.Year != 1) ?
                            ((item.RequiredDate.Year > orderDTO.MinRequiredDate.Year) ||
                            (item.RequiredDate.Year == orderDTO.MinRequiredDate.Year && item.RequiredDate.Month > orderDTO.MinRequiredDate.Month) ||
                            (item.RequiredDate.Year == orderDTO.MinRequiredDate.Year && item.RequiredDate.Month == orderDTO.MinRequiredDate.Month && item.RequiredDate.Day > orderDTO.MinRequiredDate.Day))
                            : true)
                          //   &&
                          //((orderDTO.MaxOrderDate.Year != 1) ?
                          //  ((item.OrderDate.Year < orderDTO.MaxOrderDate.Year) ||
                          //  (item.OrderDate.Year == orderDTO.MaxOrderDate.Year && item.OrderDate.Month < orderDTO.MaxOrderDate.Month) ||
                          //  (item.OrderDate.Year == orderDTO.MaxOrderDate.Year && item.OrderDate.Month == orderDTO.MaxOrderDate.Month && item.OrderDate.Day < orderDTO.MaxOrderDate.Day))
                          //  : true)
                     select item;
            
            return orders;
        }

        // GET: api/Orders/5
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "Order", ClaimValue = "Get")]
#endif
        [ResponseType(typeof(Order))]
        public IHttpActionResult GetOrder(int id)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // PUT: api/Orders/5
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "Order", ClaimValue = "Put")]
#endif
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrder(int id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.Id)
            {
                return BadRequest();
            }

            db.Entry(order).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST: api/Orders
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "Order", ClaimValue = "Post")]
#endif
        [ResponseType(typeof(Order))]
        public IHttpActionResult PostOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Orders.Add(order);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = order.Id }, order);
        }

        // DELETE: api/Orders/5
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "Order", ClaimValue = "Delete")]
#endif
        [ResponseType(typeof(Order))]
        public IHttpActionResult DeleteOrder(int id)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            db.Orders.Remove(order);
            db.SaveChanges();

            return Ok(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(int id)
        {
            return db.Orders.Count(e => e.Id == id) > 0;
        }
    }
}