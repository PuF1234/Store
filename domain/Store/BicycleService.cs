using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store
{
    public class BicycleService
    {
        private readonly IBicycleRepos bicycleRepos;

        public  BicycleService(IBicycleRepos bicycleRepos)
        {
            this.bicycleRepos = bicycleRepos;
        }

        public Bicycle[] GetAllByQuery(string query)
        {
            if(Bicycle.IsSerial(query))           
                return bicycleRepos.GetAllBySerialNumber(query);
            
            return bicycleRepos.GetAllByTitleOrProducer(query);
        }
    }
}
