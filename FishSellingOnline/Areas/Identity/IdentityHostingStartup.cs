using System;
using FishSellingOnline.Areas.Identity.Data;
using FishSellingOnline.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(FishSellingOnline.Areas.Identity.IdentityHostingStartup))]
namespace FishSellingOnline.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<FishSellingOnlineContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("FishSellingOnlineContextConnection")));

                services.AddDefaultIdentity<FishSellingOnlineUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<FishSellingOnlineContext>();
            });
        }
    }
}