using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace se_CodeFirst_3.Models
{
    public class Supplier
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "نام تولید کننده نمی تواند خالی باشد.")]
        [Display(Name = "نام")]
        public string Name { get; set; }

        [Required, Display(Name = "نام شرکت")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "شماره تلفن نمی تواند خالی باشد.")]
        [Display(Name = "تلفن")]
        public int PhoneNumber { get; set; }

        [Display(Name = "آدرس")]
        public string Address { get; set; }

        [DefaultValue(false)]
        [Display(Name = "حذف شده؟")]
        public bool IsDeleted { get; set; }

        //Navigation Properties:
        public virtual ICollection<Product> Products { get; set; }

    }
}