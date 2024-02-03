﻿namespace Store.Data
{
    public class OrderItemDto
    {
        public int Id { get; set; }

        public int BicycleId { get; set; }

        public decimal Price { get; set; }

        public int Count { get; set; }

        public OrderDto Order { get; set; }
    }
}
