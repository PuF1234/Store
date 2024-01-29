namespace Store
{
    public class OrderItem
    {
        public int BicycleId {  get; }

        private int count;

        public int Count
        {
            get { return count; }
            set 
            {
                ThrowIfInvalidCount(value);

                count = value;
            }
        }

        public decimal Price { get; }

        public OrderItem(int bicycleId, int count, decimal price)
        {
            ThrowIfInvalidCount(count); 
            BicycleId = bicycleId;
            Count = count;
            Price = price;
        }

        private static void ThrowIfInvalidCount(int count)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException("Count must be greather than 0!");
        }
    }
}
