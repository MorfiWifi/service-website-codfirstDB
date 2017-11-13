namespace se_CodeFirst_3.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using Helper;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<se_CodeFirst_3.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;// true;
        }

        protected override void Seed(se_CodeFirst_3.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            //var suppliers = new List<Supplier>
            //{
            //    new Supplier { Name = "a", Phone = 11, CompanyName = "a", Address = "A" },
            //    new Supplier { Name = "a2", Phone = 12, CompanyName = "a2", Address = "A2" },
            //    new Supplier { Name = "a3", Phone = 13, CompanyName = "a3", Address = "A3" },
            //    new Supplier { Name = "a4", Phone = 14, CompanyName = "a4", Address = "A4" }
            //};

            //suppliers.ForEach(item => context.Suppliers.Add(item));
            //context.SaveChanges();

            //var roleStore = new RoleStore<IdentityRole>(context);
            //var roleManager = new RoleManager<IdentityRole>(roleStore);

            //await roleManager.CreateAsync(new IdentityRole { Name = "Administrator" });

            //var userStore = new UserStore<ApplicationUser>(context);
            //var userManager = new UserManager<ApplicationUser>(userStore);

            //var user = new ApplicationUser { UserName = "admin" };
            //await userManager.CreateAsync(user);
            //await userManager.AddToRoleAsync(user.Id, "Administrator");

            //UsefulMethodsHelper methodsHelper = new UsefulMethodsHelper();
            ////methodsHelper.InitializationOfUsersPasswordAndRoles(context);

            //methodsHelper.FillClaimsTable(context);


        }
    }
}
