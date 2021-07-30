using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace FishSellingOnline.Areas.Identity.Data
{
    public enum Roles
    {
        Admin,
        Seller,
        Customer
    }
    public class ContextRoles
    {
        public static async Task SeedRolesAsync(UserManager<FishSellingOnlineUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Seller.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Customer.ToString()));
        }
    }
}
