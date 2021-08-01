using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FishSellingOnline.Models
{
    public class Cart
    {
        public int CartID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public byte[] ProductImage { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }
        public int Total { get; set; }
    }
}
