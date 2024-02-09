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

        public async Task<BicycleModel> GetByIdAsync(int id)
        {
            var bicycle = await bicycleRepository.GetByIdAsync(id);

            return Map(bicycle);
        }

        public async Task<IReadOnlyCollection<BicycleModel>> GetAllByQueryAsync(string query)
        {
            var bicycles = Bicycle.IsSerial(query)
                      ? await bicycleRepository.GetAllBySerialNumberAsync(query)
                      : await bicycleRepository.GetAllByTitleOrProducerAsync(query);

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
