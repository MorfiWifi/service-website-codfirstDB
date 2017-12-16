using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace se_CodeFirst_3.Models
{
    public class CustomerViewModel
    {
        [Display(Name = "نام مشتری")]
        public string Name { get; set; }

        [Display(Name = "نام شرکت")]
        public string CompanyName { get; set; }

        [Display(Name = "شماره تماس")]
        public int PhoneNumber { get; set; }
        public ICollection<Order> Orders { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Order_Detail> Order_Details { get; set; }

        [Display(Name = "قیمت کل خریدها")]
        public double PriceOfAllProductsPurchased { get; set; }

        [Display(Name = "تعداد کل خریدها")]
        public int AllProductsPurchased { get; set; }
    }
}