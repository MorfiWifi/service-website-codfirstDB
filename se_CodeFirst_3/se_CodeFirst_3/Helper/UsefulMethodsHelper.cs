using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using se_CodeFirst_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace se_CodeFirst_3.Helper
{
    public class UsefulMethodsHelper
    {
        public void InitializationOfUsersPasswordAndRoles(ApplicationDbContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            //this is our first 4 roles in database:
            roleManager.Create(new IdentityRole
            {
                Name = "Administrator"
            });

            roleManager.Create(new IdentityRole
            {
                Name = "Accountant"
            });

            roleManager.Create(new IdentityRole
            {
                Name = "Secretary"
            });

            roleManager.Create(new IdentityRole
            {
                Name = "StoreKeeper"
            });


            var admin = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@gmail.com"
            };

            var accountant = new ApplicationUser
            {
                UserName = "accountant",
                Email = "accountant@gmail.com"
            };

            var secretary = new ApplicationUser
            {
                UserName = "secretary",
                Email = "secretary@gmail.com"
            };

            var storeKeeper = new ApplicationUser
            {
                UserName = "storekeeper",
                Email = "storekeeper@gmail.com"
            };

            //admin.Salary = 5000;
            //secretary.Salary = 4000;
            //accountant.Salary = 3000;
            //storeKeeper.Salary = 2000;

            userManager.Create(admin);
            userManager.Create(accountant);
            userManager.Create(secretary);
            userManager.Create(storeKeeper);

            userManager.AddToRole(admin.Id, "Administrator");
            userManager.AddToRole(accountant.Id, "Accountant");
            userManager.AddToRole(secretary.Id, "Secretary");
            userManager.AddToRole(storeKeeper.Id, "StoreKeeper");

            userManager.AddPassword(admin.Id, "bbBB11!!");
            userManager.AddPassword(accountant.Id, "bbBB11!!");
            userManager.AddPassword(secretary.Id, "bbBB11!!");
            userManager.AddPassword(storeKeeper.Id, "bbBB11!!");

            
        }


    }
}