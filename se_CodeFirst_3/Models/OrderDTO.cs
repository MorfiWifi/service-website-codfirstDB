using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace se_CodeFirst_3.Models
{
    public class OrderDTO
    {
        public string Content { get; set; }
        public string Customer { get; set; }
        public DateTime MinOrderDate { get; set; }
        public DateTime MaxOrderDate { get; set; }
        public DateTime MinRequiredDate { get; set; }
        public DateTime MaxRequiredDate { get; set; }
        
    }
}