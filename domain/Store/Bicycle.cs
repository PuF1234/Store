using System.Text.RegularExpressions;
using Store.Data;

namespace Store
{
    public class Bicycle
    {
        private readonly BicycleDto dto;

        public int ID => dto.ID;

        public string Serial_Number
        {
            get => dto.Serial_number;
            set
            {
                if(TryFormatSerialNumber(value, out string formatedSerialNumber))
                    dto.Serial_number = formatedSerialNumber;

                throw new ArgumentException(nameof(Serial_Number));
            } 
        }
        
        public string Title
        {
            get => dto.Title;
            set
            {
                if(string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(nameof(Title));
                dto.Title = value.Trim();
            }
        }


        public string Producer
        {
            get => dto.Producer;
            set => dto.Producer = value?.Trim();
        }

        public string Description
        {
            get => dto.Description;
            set => dto.Description = value?.Trim();
        }

        public decimal Price
        {
            get => dto.Price;
            set => dto.Price = value;
        }

        internal Bicycle(BicycleDto dto)
        {
            this.dto = dto;
        }

        public static bool TryFormatSerialNumber(string serialNumber, out string formatedSerialNumber)
        {
            if (serialNumber == null)
            {
                formatedSerialNumber = null;
                return false;
            }

            formatedSerialNumber = serialNumber.Replace("-", "")
                                               .Replace(" ", "")
                                               .ToUpper();

            return Regex.IsMatch(formatedSerialNumber, @"^SERIAL:\d{7}(\d{3})?$");
        }

        public static bool IsSerial(string serialNumber) 
            => TryFormatSerialNumber(serialNumber, out _);

        public static class DtoFactory
        {
            public static BicycleDto Create(string serialNumber, string producer, string title, string description, decimal price)
            {
                if(TryFormatSerialNumber(serialNumber, out string formatedSerialNumber)) 
                    serialNumber = formatedSerialNumber;
                else 
                    throw new ArgumentException(nameof(serialNumber));



                if (string.IsNullOrWhiteSpace(title))
                    throw new ArgumentException(nameof(title));

                return new BicycleDto
                {
                    Serial_number = serialNumber,
                    Producer = producer?.Trim(),
                    Title = title.Trim(),
                    Description = description?.Trim(),
                    Price = price
                };
            }
        }

        public static class Mapper
        {
            public static Bicycle Map (BicycleDto dto) => new Bicycle(dto);

            public static BicycleDto Map(Bicycle domain) => domain.dto;
        }
        
    }
}