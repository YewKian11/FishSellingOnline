using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using FishSellingOnline.Data;
using FishSellingOnline.Models;
using FishSellingOnline.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FishSellingOnline
{
    public class Program
    {
        //Change void to async 
        public static async Task Main(string[] args)
        {

            var host = CreateHostBuilder(args).Build();
                //invoke the initialize function in  XXX Data class 
              using (var scope = host.Services.CreateScope()) 
              {
                var services = scope.ServiceProvider;
                //try catch Roles
                try
                {
                    //link db
                    var context = services.GetRequiredService<FishSellingOnlineContext>();
                    // always make migration done before call initialiize()
                  context.Database.Migrate();
                    var userManager = services.GetRequiredService<UserManager<FishSellingOnlineUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    await ContextRoles.SeedRolesAsync(userManager, roleManager);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();

                    logger.LogError(ex, "An error occured sending the DB.");
                }
              
                }
                  host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
