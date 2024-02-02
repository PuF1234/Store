﻿using System.Collections;

namespace Store
{
    public class OrderItemCollection : IReadOnlyCollection<OrderItem>
    {

        private readonly List<OrderItem> items;

        public OrderItemCollection(IEnumerable<OrderItem> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            this.items = new List<OrderItem>(items);
        }

        public int Count => items.Count;

        public IEnumerator<OrderItem> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (items as IEnumerable).GetEnumerator();
        }

        public OrderItem Get(int bicycleId)
        {
            if(TryGet(bicycleId, out OrderItem orderItem))
                return orderItem;

            throw new InvalidOperationException("Bicycle not found");
        }

        public bool TryGet(int bicycleId, out OrderItem orderItem)
        {
            var index = items.FindIndex(item => item.BicycleId == bicycleId);
            if (index == -1)
            {
                orderItem = null;
                return false;
            }

            orderItem = items[index];
            return true;
        }

        public OrderItem Add(int bicycleId, decimal price, int count)
        {
            if (TryGet(bicycleId, out OrderItem orderItem))
                throw new InvalidOperationException("Bicycle already exists.");

            orderItem = new OrderItem(bicycleId, price, count);
            items.Add(orderItem);

            return orderItem;
        }

        public void Remove(int bicycleId)
        {
            items.Remove(Get(bicycleId));
        }
    }
}
