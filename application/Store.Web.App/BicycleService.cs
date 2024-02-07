using System.Data;

namespace Store.Web.App
{
    public class BicycleService
    {
        private readonly IBicycleRepository bicycleRepository;

        public BicycleService(IBicycleRepository bicycleRepository)
        {
            this.bicycleRepository = bicycleRepository;
        }

        public BicycleModel GetById(int id)
        {
            var bicycle = bicycleRepository.GetByIds(id);

            return Map(bicycle);
        }

        public IReadOnlyCollection<BicycleModel> GetAllByQuery(string query)
        {
            var bicycles = Bicycle.IsSerial(query)
                      ? bicycleRepository.GetAllBySerialNumber(query)
                      : bicycleRepository.GetAllByTitleOrProducer(query);

            return bicycles.Select(Map)
                           .ToArray();
        }

        private BicycleModel Map(Bicycle bicycle)
        {
            return new BicycleModel
            {
                Id = bicycle.ID,
                SerialNumber = bicycle.Serial_Number,
                Title = bicycle.Title,
                Producer = bicycle.Producer,
                Description = bicycle.Description,
                Price = bicycle.Price,
            };
        }
    }
}
