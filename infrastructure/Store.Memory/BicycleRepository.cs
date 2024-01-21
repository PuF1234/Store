using System;
using System.Linq;

namespace Store.Memory
{
    public class BicycleRepository : IBicycleRepos
    {
        private readonly Bicycle[] bicycles = new[]
        {
            new Bicycle(1, "Tsunami snm-100", "Serial: 1122334", "Tsunami",
                "Fixie Fixed Gear Bike Tisunami Single Speed Bicycle\r\n\r\nFrame: Aluminum Alloy\r\n\r\nFork: Aluminum Alloy\r\n\r\nSize: 49/52/55/58cm\r\n\r\nFit Height: 163cm-190cm\r\n\r\nWeight: 9.8kg\r\n\r\nWheel Size: 700C\r\n\r\nBrake: Fits V-brake(Not include)\r\n\r\nAll of This Bicycle Cycling Parts are Customizable",
                452.79m),
            new Bicycle(2, "6ku Urban Track", "Serial: 1111111", "6ku",
                "Do you want a lightweight track bike that won’t break the bank? Then the 6KU Urban Track is the bike for you. Our Urban Track features a lightweight aluminum frame and fork. This is one purchase you won’t regret.\r\n\r\nAll 6KU Urban Track bikes include FREE assembly tools. $30 Value of free tools that is all you will need to assemble your new bike!\r\n\r\n \r\n\r\nLightweight Full Aluminum Frame and Fork\r\n30mm Deep V Double-Walled Alloy Wheels\r\nRide Fixed Gear or Freewheel with a Flip-Flop Hub\r\nEasy Maintenance and Upkeep\r\n30-Day Hassle-Free Return Policy",
                259.99m),
            new Bicycle(3, "6ku Fixie BIke", "Serial: 7777777", "6ku",
                "Looking for a good bike at a low price? Then the 6KU Fixie is what you’re looking for. It’s the dream single-speed bike that is well-built, sturdy, and ideal for short commutes. Buy it, ride it, and we promise you’ll have a smile on your face. There is no other fixie out there like the 6KU.  \r\n\r\nAll 6KU Fixies come with a $30 value of FREE tools included with every bike purchase.\r\n\r\nComfortable Steel Frame\r\nReliable Front and Rear Brakes\r\nFixed or Freewheel with a Flip-Flop Hub\r\nEasy Maintenance and Upkeep\r\n30-Day Hassle-Free Return Policy",
                249.00m),
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

        public Bicycle GetById(int id)
        {
            return bicycles.Single(bicycle => bicycle.ID == id);
        }
    }
}
