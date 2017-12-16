using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace se_CodeFirst_3.Models
{
    public class ProductDTO
    {
        public string Name { get; set; }
        public int MinUnitPrice { get; set; }
        public int MaxUnitPrice { get; set; }
        public int MinSellUnitPrice { get; set; }
        public int MaxSellUnitPrice { get; set; }
        public int MinUnitsInStock { get; set; }
        public int MaxUnitsInStock { get; set; }
        public string SupplierCompanyName { get; set; }
    }
}