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

        public int TotalCount => items.Sum(item => item.Count);        

        public decimal TotalPrice => items.Sum(item => item.Price * item.Count)
                                     + (Delivery?.Amount ?? 0m);     
        
        public string CellPhone { get; set; }

        public OrderDelivery Delivery { get; set; } 

        public OrderPayment Payment { get; set; }

        public Order(int id, IEnumerable<OrderItem> items)
        {
            if(items == null) 
                throw new ArgumentNullException(nameof(items));

            Id = id;

            this.items = new List<OrderItem>(items);
        }

        public OrderItem GetItem(int bicycleId)
        {
            int index = items.FindIndex(item => item.BicycleId == bicycleId);

            if (index == -1)
                throw new InvalidOperationException("Item not found!");
            
            return items[index];
        }

        
        public void AddOrUpdateItem(Bicycle bicycle, int count)
        {
            if(bicycle == null)
                throw new ArgumentNullException(nameof(bicycle));

            int index = items.FindIndex(item => item.BicycleId == bicycle.ID);

            if(index == -1)
                items.Add(new OrderItem(bicycle.ID, count, bicycle.Price));
            else
                items[index].Count += count;
        }
        
        public void RemoveItem(int bicycleId)
        {            
            int index = items.FindIndex(item => item.BicycleId == bicycleId);

            if (index == -1)
                throw new InvalidOperationException("Order doesn't contain specified item.");

            items.RemoveAt(index);
        }
    }
}
