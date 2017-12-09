using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace se_CodeFirst_3.Models
{
    public class SupplierInformationViewModel
    {
        [ Display(Name = "نام")]
        public string Name { get; set; }

        [Display(Name = "نام شرکت")]
        public string CompanyName { get; set; }

        [Display(Name = "تلفن")]
        public int PhoneNumber { get; set; }

        [Display(Name = "آدرس")]
        public string Address { get; set; }

        public ICollection<Product> Products { get; set; }
        [Display(Name = "موجودی انبار")]
        public int UnitsInStock { get; set; }

        [Display(Name = "قیمت")]
        public double Price { get; set; }

    }
}