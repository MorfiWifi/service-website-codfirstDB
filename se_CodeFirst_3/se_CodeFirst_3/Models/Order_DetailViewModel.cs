using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace se_CodeFirst_3.Models
{
    //TODO: Set This:
    public class OrderViewModel
    {
        [Display(Name = "قیمت واحد")]
        public int UnitPrice { get; set; }

        [Display(Name = "تعداد")]
        public int Quantity { get; set; }

        //[Display(Name = "تخفیف")]
        //public int Discount { get; set; }
        public Product Product { get; set; }
        public Order Order { get; set; }
        public Customer Customer { get; set; }

    }
}