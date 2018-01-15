using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace se_CodeFirst_3.Models
{
    public class StatisticsViewModel
    {
        public int Id { get; set; }

        
        //Hole Company:
        [Display(Name = "کل فروش")]
        public double AllSells { get; set; }

        [Display(Name = "کل خرید")]
        public double AllBuys { get; set; }

        [Display(Name = "کل سود")]
        public double AllProfits { get; set; }

        [Display(Name = "فروش ماه گذشته")]
        public double LastMonthSells { get; set; }

        [Display(Name = "خرید ماه گذشته")]
        public double LastMonthBuys { get; set; }

        [Display(Name = "سود ماه گذشته")]
        public double LastMonthProfitss { get; set; }

        [Display(Name = "فروش سال گذشته")]
        public double LastYearSells { get; set; }

        [Display(Name = "خرید سال گذشته")]
        public double LastYearBuys { get; set; }

        [Display(Name = "سود سال گذشته")]
        public double LastYearProfits { get; set; }

        //
        [Display(Name = "فروش دوره مشخص شده")]
        public double CustomDateSells { get; set; }

        [Display(Name = "خرید دوره مشخش شده")]
        public double CustomDateBuys { get; set; }

        [Display(Name = "سود دوره مشخص شده")]
        public double CustomDateProfits { get; set; }

        //Users:

        [Display(Name = "تعداد کل کاربران شرکت")]
        public int UserCounts { get; set; }

        [Display(Name = "حقوق پرداختی به کاربران")]
        public double UserSalaries { get; set; }

        //Suppliers:
        [Display(Name = "تعداد کل تولیدکنندگان")]
        public int SuppliersCount { get; set; }
        public virtual ICollection<SupplierViewModel2> Suppliers { get; set; }

        //Customers:
        [Display(Name = "تعداد کل مشتری ها")]
        public int CustomersCount { get; set; }
        public virtual ICollection<CustomerViewModel2> Customers { get; set; }

    }

    

    public class SupplierViewModel2
    {
        public int Id { get; set; }
        [Display(Name = "نام شرکت تولیدکننده")]
        public string SupplierCompanyName { get; set; }
        [Display(Name = "تعداد کالاها")]
        public int ProductsCount { get; set; }
        [Display(Name = "کل پول صرف شده")]
        public double MoneySpent { get; set; }
    }

    public class CustomerViewModel2
    {
        public int Id { get; set; }
        [Display(Name = "نام مشتری")]
        public string CustomerName { get; set; }
        [Display(Name = "تعداد کالاها")]
        public int ProductsCount { get; set; }
        [Display(Name = "سود به دست آمده")]
        public double ProfitsGained { get; set; }
    }
}