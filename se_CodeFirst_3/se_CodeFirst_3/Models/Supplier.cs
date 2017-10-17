using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace se_CodeFirst_3.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Phone { get; set; }

        //Navigation Properties:
        public virtual ICollection<Product> Products { get; set; }

    }
}