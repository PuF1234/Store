﻿namespace Store
{
    public class OrderDelivery
    {
        public string UniqueCode { get; }

        public string Description { get; }

        public decimal Price { get; }

        public IReadOnlyDictionary<string, string> Parameters { get; }

        public OrderDelivery(string uniqueCode, string description, decimal price, IReadOnlyDictionary<string, string> parameters)
        {
            if (string.IsNullOrEmpty(uniqueCode))            
                throw new ArgumentNullException(nameof(uniqueCode));
            
            if(string.IsNullOrEmpty(description))
                throw new ArgumentNullException(nameof(description));

            if(parameters == null) 
                throw new ArgumentNullException(nameof(parameters));

            UniqueCode = uniqueCode;
            Description = description;
            Price = price;
            Parameters = parameters;
        }
    }
}
