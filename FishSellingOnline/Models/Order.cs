using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FishSellingOnline.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public string UserName { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }
}
