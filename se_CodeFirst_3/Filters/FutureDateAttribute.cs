using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace se_CodeFirst_3.Filters
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var futureDate = value as DateTime?;
            var memberNames = new List<string>() { context.MemberName };

            if (futureDate != null)
            {
                if (futureDate.Value.Date < DateTime.UtcNow.Date)
                {
                    return new ValidationResult("تاریخ، باید تاریخ آینده باشد", memberNames);
                }
            }

            return ValidationResult.Success;
        }
    }

}