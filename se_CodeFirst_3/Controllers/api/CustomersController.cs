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
    [LogApi]
#endif
    public class CustomersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Customers
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "Customer", ClaimValue = "Get")]
#endif
        public IQueryable<Customer> GetCustomers([FromUri]Customer customer)
        {
            IQueryable<Customer> customers = from item in db.Customers
                                           where item.IsDeleted == false
                                           select item;
            customers = from item in customers
                       where
                           (!String.IsNullOrEmpty(customer.CompanyName) ? item.CompanyName.Contains(customer.CompanyName) : true) &&
                            (!String.IsNullOrEmpty(customer.Name) ? item.Name.Contains(customer.Name) : true) &&
                            (customer.PhoneNumber != 0 ? item.PhoneNumber.ToString().Contains(customer.PhoneNumber.ToString()) : true)
                        select item;

            return customers;
        }

        [Route("api/AllCustomers")]
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "Customer", ClaimValue = "Get")]
#endif
        public IQueryable<Customer> GetAllCustomers()
        {
            return db.Customers;
        }

        // GET: api/Customers/5
        [ResponseType(typeof(Customer))]
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "Customer", ClaimValue = "Get")]
#endif
        public IHttpActionResult GetCustomer(int id)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            var allProductsPurchased = (from item in customer.Orders
                                        join item2 in db.Order_Details on item.Id equals item2.OrderId
                                        select item2.Quantity).Sum();

            var priceOfAllProductsPurchased = (from item in customer.Orders
                                               join item2 in db.Order_Details on item.Id equals item2.OrderId
                                               join item3 in db.Products on item2.ProductId equals item3.Id
                                               select item3.BuyUnitPrice * item2.Quantity).Sum();

            var products = from item in customer.Orders
                           join item2 in db.Order_Details on item.Id equals item2.OrderId
                           join item3 in db.Products on item2.ProductId equals item3.Id
                           select item3;

            var order_details = from item in customer.Orders
                                join item2 in db.Order_Details on item.Id equals item2.OrderId
                                select item2;

            CustomerViewModel customerInformation = new CustomerViewModel
            {
                Name = customer.Name,
                Orders = customer.Orders,
                CompanyName = customer.CompanyName,
                PhoneNumber = customer.PhoneNumber,
                AllProductsPurchased = allProductsPurchased,
                PriceOfAllProductsPurchased = priceOfAllProductsPurchased,
                Products = products,
                Order_Details = order_details
            };

            return Ok(customerInformation);
        }

        // PUT: api/Customers/5
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "Customer", ClaimValue = "Put")]
#endif
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCustomer(int id, Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.Id)
            {
                return BadRequest();
            }

            db.Entry(customer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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

        // POST: api/Customers
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "Customer", ClaimValue = "Post")]
#endif
        [ResponseType(typeof(Customer))]
        public IHttpActionResult PostCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Customers.Add(customer);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = customer.Id }, customer);
        }

        // DELETE: api/Customers/5
#if DEBUG
#else
        [ClaimsAuthorization(ClaimType = "Customer", ClaimValue = "Delete")]
#endif
        [ResponseType(typeof(Customer))]
        public IHttpActionResult DeleteCustomer(int id)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            db.Customers.Remove(customer);
            db.SaveChanges();

            return Ok(customer);
        }

        // Delete: api/Customers/SafeDelete/5
        //[ResponseType(typeof(Customer))]
        //public IHttpActionResult SafeDeleteCustomer(int id)
        //{
        //    Customer customer = db.Customers.Find(id);
        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }

        //    customer.IsDeleted = 
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerExists(int id)
        {
            return db.Customers.Count(e => e.Id == id) > 0;
        }
    }
}