using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FishSellingOnline.Models
{
    public class Product //table name: Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public byte[] ProductImage { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public int stockleft { get; set; }
    }
}
