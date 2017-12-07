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

        //[Display(Name = "قیمت واحد")]
        //public int UnitPrice { get; set; }

        [Required(ErrorMessage = "تعداد کالا باید مشخص باشد.")]
        [Display(Name = "تعداد")]
        [Range(0, int.MaxValue, ErrorMessage = "تعداد کالاها نمی تواند منفی باشد.")]
        public int Quantity { get; set; }

        //[Display(Name = "تخفیف")]
        //public int Discount { get; set; }

        [Display(Name = "محصول")]
        public int ProductId { get; set; }

        [Display(Name = "سفارش")]
        public int OrderId { get; set; }

        //Navigation Properties:
        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }

    }
}