using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FishSellingOnline.Models;

namespace FishSellingOnline.Data
{
    public class FishSellingOnlineProductContext : DbContext
    {
        public FishSellingOnlineProductContext (DbContextOptions<FishSellingOnlineProductContext> options)
            : base(options)
        {
        }

        public DbSet<FishSellingOnline.Models.Product> Product { get; set; }

        public DbSet<FishSellingOnline.Models.Order> Order { get; set; }
    }
}
