using System;
using System.Collections.Generic;
using System.Text;

namespace scale_blu
{
    public class Order
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public double UnitWeight { get; set; }
    }
}
