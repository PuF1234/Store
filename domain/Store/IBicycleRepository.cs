using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store
{
    public interface IBicycleRepository
    {
        Task<Bicycle[]> GetAllByIdsAsync(IEnumerable<int> bicycleIds);

        Task<Bicycle[]> GetAllBySerialNumberAsync(string serialNumber);

        Task<Bicycle[]> GetAllByTitleOrProducerAsync(string TitleOrProducer);

        Task<Bicycle> GetByIdAsync(int id);
    }
}
