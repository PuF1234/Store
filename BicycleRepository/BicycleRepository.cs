using Store;

namespace BicycleRepository
{
    public class BicycleRepository : IBicycleRepository
    {
        public Bicycle[] GetAllByIds(IEnumerable<int> bicycleIds)
        {
            throw new NotImplementedException();
        }

        public Bicycle[] GetAllBySerialNumber(string serialNumber)
        {
            throw new NotImplementedException();
        }

        public Bicycle[] GetAllByTitleOrProducer(string TitleOrProducer)
        {
            throw new NotImplementedException();
        }

        public Bicycle GetByIds(int id)
        {
            throw new NotImplementedException();
        }
    }
}
