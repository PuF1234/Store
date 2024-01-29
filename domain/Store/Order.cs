using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store
{
    public class Order
    {
        public int Id { get; } 

        private List<OrderItem> items;
        
        public IReadOnlyCollection<OrderItem> Items 
        {
            get { return items; } 
        }

        public int TotalCount
        {
            get { return items.Sum(item => item.Count); }
        }

        public decimal TotalPrice
        {
            get { return items.Sum(item => item.Price * item.Count); }
        }

        public Order(int id, IEnumerable<OrderItem> items)
        {
            if(items == null) 
                throw new ArgumentNullException(nameof(items));

            Id = id;

            this.items = new List<OrderItem>(items);
        }

        public void AddItem(Bicycle bicycle, int count)
        {
            if(bicycle == null)
                throw new ArgumentNullException(nameof(bicycle));

            var item = items.SingleOrDefault(x => x.BicycleId == bicycle.ID);

            if(item == null)
            {
                items.Add(new OrderItem(bicycle.ID, count, bicycle.Price));
            }
            else
            {
                items.Remove(item);
                items.Add(new OrderItem(bicycle.ID, item.Count + count, bicycle.Price));
            }
        }
    }
}
