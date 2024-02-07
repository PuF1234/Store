using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.EF
{
    public class BicycleRepository : IBicycleRepository
    {
        private readonly DbContextFactory dbContextFactory;

        public BicycleRepository(DbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public Bicycle[] GetAllByIds(IEnumerable<int> bicycleIds)
        {
            var dbContext = dbContextFactory.Create(typeof(BicycleRepository));

            return dbContext.Bicycles
                            .Where(bicycle => bicycleIds.Contains(bicycle.ID))
                            .AsEnumerable()
                            .Select(Bicycle.Mapper.Map)
                            .ToArray();
        }

        public Bicycle[] GetAllBySerialNumber(string serialNumber)
        {
            var dbContext = dbContextFactory.Create(typeof(BicycleRepository));

            if (Bicycle.TryFormatSerialNumber(serialNumber, out string formattedSerialNumber))
            {
                return dbContext.Bicycles
                                .Where(bicycle => bicycle.Serial_number == formattedSerialNumber)
                                .AsEnumerable()
                                .Select(Bicycle.Mapper.Map)
                                .ToArray();
            }

            return new Bicycle[0];
        }

        public Bicycle[] GetAllByTitleOrProducer(string titleOrProducer)
        {
            var dbContext = dbContextFactory.Create(typeof(BicycleRepository));

            var parameter = new SqlParameter("@titleOrProducer", titleOrProducer);
            return dbContext.Bicycles
                            .FromSqlRaw("SELECT * FROM Bicycles WHERE CONTAINS((Producer, Title), @titleOrProducer)",
                                        parameter)
                            .AsEnumerable()
                            .Select(Bicycle.Mapper.Map)
                            .ToArray();
        }

        public Bicycle GetByIds(int id)
        {
            var dbContext = dbContextFactory.Create(typeof(BicycleRepository));

            var dto = dbContext.Bicycles
                               .Single(bicycle => bicycle.ID == id);

            return Bicycle.Mapper.Map(dto);
        }
    }
}
