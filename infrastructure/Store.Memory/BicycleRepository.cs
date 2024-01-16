using System;
using System.Linq;

namespace Store.Memory
{
    public class BicycleRepository : IBicycleRepos
    {
        private readonly Bicycle[] bicycles = new[]
        {
            new Bicycle(1, "Tsunami snm-100"),
            new Bicycle(2, "6ku Urban Track"),
            new Bicycle(3, "Riptide"),
        };
        public Bicycle[] GetAllByTitle(string titlePart)
        {
            return bicycles.Where(bicycle => bicycle.Title.Contains(titlePart))
                .ToArray();
        }
    }
}
