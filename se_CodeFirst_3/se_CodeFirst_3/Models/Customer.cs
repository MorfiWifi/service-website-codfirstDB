using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace se_CodeFirst_3.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "نام مشتری")]
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public int Phone { get; set; }
        //Navigation Properties:
        public virtual ICollection<Order> Orders { get; set; }
    }
}