using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store
{
    public interface IBicycleRepos
    {
        Bicycle[] GetAllByIds(IEnumerable<int> bicycleIds);
        Bicycle[] GetAllBySerialNumber(string serialNumber);

        Bicycle[] GetAllByTitleOrProducer(string TitleOrProducer);

        Bicycle GetByIds(int id);
    }
}
