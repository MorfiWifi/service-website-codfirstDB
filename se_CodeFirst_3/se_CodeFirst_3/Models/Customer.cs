using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace se_CodeFirst_3.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "نام مشتری نمی تواند خالی باشد.")]
        [Display(Name = "نام مشتری")]
        public string Name { get; set; }

        [Display(Name = "نام شرکت")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "شماره تماس نمی تواند خالی باشد.")]
        [Display(Name = "شماره تماس")]
        public int PhoneNumber { get; set; }

        //Navigation Properties:
        public virtual ICollection<Order> Orders { get; set; }
    }
}