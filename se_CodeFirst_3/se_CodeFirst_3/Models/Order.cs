using se_CodeFirst_3.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace se_CodeFirst_3.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "تاریخ سفارش نمی تواند خالی باشد.")]
        [Display(Name = "تاریخ سفارش")]
        [FutureDate]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "تاریخ تحویل نمی تواند خالی باشد.")]
        [Display(Name = "تاریخ تحویل")]
        [FutureDate]//this is a custom validation Exists in Filters Folder
        public DateTime RequiredDate { get; set; }

        [Display(Name = "مشتری")]
        public int? CustomerId { get; set; }

        [Display(Name = "قرارداد")]
        public int? ContractId { get; set; }


        //Navigation Properties:
        public virtual ICollection<Order_Detail> Order_Details { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Contract Contract { get; set; }
    }
}