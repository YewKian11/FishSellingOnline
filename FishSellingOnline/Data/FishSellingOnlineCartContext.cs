using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FishSellingOnline.Models;

namespace FishSellingOnline.Data
{
    public class FishSellingOnlineCartContext : DbContext
    {
        public FishSellingOnlineCartContext(DbContextOptions<FishSellingOnlineCartContext> options)
            : base(options)
        {

        }
        public DbSet<FishSellingOnline.Models.Cart> Cart { get; set; }
    }
}
