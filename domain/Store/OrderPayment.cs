namespace Store
{
    public class OrderPayment
    {
        public string UniqueCode { get; }

        public string Description { get; }

        public IReadOnlyDictionary<string, string> Parameters { get; }

        public OrderPayment(string uniqueCode, string description, IReadOnlyDictionary<string, string> parameters)
        {
            if (string.IsNullOrEmpty(uniqueCode))            
                throw new ArgumentNullException(nameof(uniqueCode));
            
            if(string.IsNullOrEmpty(description))
                throw new ArgumentNullException(nameof(description));

            if(parameters == null) 
                throw new ArgumentNullException(nameof(parameters));

            UniqueCode = uniqueCode;
            Description = description;
            Parameters = parameters;
        }
    }
}
