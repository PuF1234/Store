using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store
{
    public class OrderItem
    {
        public int BicycleId {  get; set; }

        public int Count { get; set; }

        public decimal Price { get; set; }

        public OrderItem(int bicycleId, int count, decimal price) 
        {
            if(count <= 0)
                throw new ArgumentOutOfRangeException("Count must be greather than 0!");
            BicycleId = bicycleId;
            Count = count;
            Price = price;
        }
    }
}
