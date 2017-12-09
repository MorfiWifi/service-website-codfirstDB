using se_CodeFirst_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace se_CodeFirst_3.Controllers.api
{
#if DEBUG

#else
    [Authorize(Roles = "Administrator,Secretary,StoreKeeper,Accountant")]
#endif
    public class OtherFunctionsController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        //Get: api/OtherFunctions/5
        public IHttpActionResult GetInformationAboutSupplier(int id)
        {
            var supplier = db.Suppliers.Find(id);

            var unitsInStock = (from item in supplier.Products
                                select item.UnitsInStock).Sum();

            var price = (from item in supplier.Products
                         select item.UnitsInStock * item.BuyUnitPrice).Sum();

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

        //[Route("api/Settings/settingName/settingValue")]
        //public IHttpActionResult PostSetting(string settingName, string settingValue)
        //{

        //}
    }
}
