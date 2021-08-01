using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FishSellingOnline.Areas.Identity.Data
{ 

    //Identify the roles
    public enum Roles{ 
    Customer,
    Seller,
    Admin
    
    }
     
    public class ContextRoles
    {
        public static async Task SeedRolesAsync(UserManager<FishSellingOnlineUser 
            > userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Roles.Customer.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Seller.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
          
        }
    }
}
