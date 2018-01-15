using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace se_CodeFirst_3.Models
{
    public class Order_DetailDTO
    {
        public string Name { get; set; }
        public int MinQuantity { get; set; }
        public int MaxQuantity { get; set; }
    }
}