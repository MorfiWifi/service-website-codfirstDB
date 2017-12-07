using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace se_CodeFirst_3.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public int QuantityPerUnit { get; set; }
        public int UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public int UnitsOnOrder { get; set; }


        //Navigation Properties:
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<Order_Detail> Order_Details { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}