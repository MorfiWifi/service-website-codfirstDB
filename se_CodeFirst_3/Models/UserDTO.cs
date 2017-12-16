using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace se_CodeFirst_3.Models
{
    public class UserDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public int MinSalary { get; set; }
        public int MaxSalary { get; set; }
        public int MinAbsentDays { get; set; }
        public int MaxAbsentDays { get; set; }
        public int MinOvertime { get; set; }
        public int MaxOvertime { get; set; }
        public int MinBenefits { get; set; }
        public int MaxBenefits { get; set; }
        
    }
}