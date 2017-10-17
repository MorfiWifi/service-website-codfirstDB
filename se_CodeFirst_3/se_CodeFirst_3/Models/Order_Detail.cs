using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace se_CodeFirst_3.Models
{
    public class Order_Detail
    {
        public int Id { get; set; }
        
        public int UnitPrice { get; set; }
        public int Quantity { get; set; }
        public int Discount { get; set; }

        //Navigation Properties:
        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }

    }
}