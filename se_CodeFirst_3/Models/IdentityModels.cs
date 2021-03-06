﻿using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Collections;
using System.Collections.Generic;
using se_CodeFirst_3.Models;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace se_CodeFirst_3.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

        [Display(Name = "حقوق")]
        [Range(0, int.MaxValue, ErrorMessage = "حقوق نمی تواند منفی باشد.")]
        public int Salary { get; set; }

        [Display(Name = "غیبت")]
        [Range(0, int.MaxValue, ErrorMessage = "روز های غیبت نمی تواند منفی باشد.")]
        public int AbsentDays { get; set; }

        [Display(Name = "اضافه کاری")]
        [Range(0, int.MaxValue, ErrorMessage = "اضافه کاری نمی تواند منفی باشد.")]
        public int OverTime { get; set; }

        [Display(Name = "مزایا")]
        [Range(0, int.MaxValue, ErrorMessage = "مزایا نمی تواند منفی باشد.")]
        public int Benefits { get; set; }


        //Navigation Properties:
        public virtual ICollection<IncomingCall> IncomingCalls { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Product> Products { get; set; }

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }


        public DbSet<Customer> Customers { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<IncomingCall> IncomingCalls { get; set; }
        public DbSet<Order_Detail> Order_Details { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<ClaimViewModel> Claims { get; set; }
        public DbSet<Log> Logs { get; set; }


        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //Important:: Here we implement Cascade Delete with entity framwork and fluent Api, so if a row in parent table
        //deleted it must delete rows related to it by foreign key in child table::
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.Entity<Order>()
        //        .HasOptional(item => item.Order_Details)
        //        .WithOptionalDependent()
        //        .WillCascadeOnDelete(true);
        //}

    }
}