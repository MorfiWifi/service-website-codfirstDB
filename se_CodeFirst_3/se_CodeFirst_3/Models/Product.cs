using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace se_CodeFirst_3.Models
{
    public class Product
    {
        [Display(Name = "شماره محصول")]
        public int Id { get; set; }

        [Required(ErrorMessage = "نام کالا باید مشخص باشد.")]
        [Display(Name="نام کالا")]
        public string Name { get; set; }

        //public int QuantityPerUnit { get; set; }

        [Required(ErrorMessage = "قیمت خرید کالا نمی تواند خالی باشد.")]
        [Display(Name = "قیمت خرید")]
        [Range(0, int.MaxValue, ErrorMessage = "قیمت خرید نمی تواند منفی باشند.")]
        public int BuyUnitPrice { get; set; }

        [Required(ErrorMessage = "قیمت فروش کالا نمی تواند خالی باشد.")]
        [Display(Name = "قیمت فروش")]
        [Range(0, int.MaxValue, ErrorMessage = "قیمت فروش نمی تواند منفی باشند.")]
        public int SellUnitPrice { get; set; }

        [Required(ErrorMessage = "تعداد کالاها باید مشخص باشد.")]
        [Display(Name = "موجودی انبار")]
        [Range(0, int.MaxValue, ErrorMessage = "تعدا کالاها نمی تواند منفی باشند.")]
        public int UnitsInStock { get; set; }

        //public int UnitsOnOrder { get; set; }

        [Display(Name = "تولید کننده")]
        public int SupplierId { get; set; }

        //Navigation Properties:
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<Order_Detail> Order_Details { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}