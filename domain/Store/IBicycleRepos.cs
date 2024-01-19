using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store
{
    public interface IBicycleRepos
    {
        Bicycle[] GetAllBySerialNumber(string serialNumber);
        Bicycle[] GetAllByTitleOrProducer(string TitleOrProducer);
    }
}
