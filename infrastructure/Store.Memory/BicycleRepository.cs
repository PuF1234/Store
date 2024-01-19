using System;
using System.Linq;

namespace Store.Memory
{
    public class BicycleRepository : IBicycleRepos
    {
        private readonly Bicycle[] bicycles = new[]
        {
            new Bicycle(1, "Tsunami snm-100", "Serial: 1122334", "Tsunami"),
            new Bicycle(2, "6ku Urban Track", "Serial: 1111111", "6ku"),
            new Bicycle(3, "Riptide", "Serial: 7777777", "Bear"),
        };

        public Bicycle[] GetAllBySerialNumber(string serialNumber)
        {
            return bicycles.Where(bicycle => bicycle.Serial_number == serialNumber)
                .ToArray();    
        }

        public Bicycle[] GetAllByTitle(string query )
        {
            throw new NotImplementedException();
        }

        public Bicycle[] GetAllByTitleOrProducer(string query)
        {
            return bicycles.Where(bicycle => bicycle.Producer.Contains(query)
                                          || bicycle.Title.Contains(query))
                .ToArray();
        }
    }
}
