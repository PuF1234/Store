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

        public async Task<Bicycle[]> GetAllByIdsAsync(IEnumerable<int> bicycleIds)
        {
            var dbContext = dbContextFactory.Create(typeof(BicycleRepository));

            var dtos = await dbContext.Bicycles
                            .Where(bicycle => bicycleIds.Contains(bicycle.ID))
                            .ToArrayAsync();
                            

            return dtos.Select(Bicycle.Mapper.Map).ToArray();
        }

        public async Task<Bicycle[]> GetAllBySerialNumberAsync(string serialNumber)
        {
            var dbContext = dbContextFactory.Create(typeof(BicycleRepository));

            if (Bicycle.TryFormatSerialNumber(serialNumber, out string formattedSerialNumber))
            {
                var dtos = await dbContext.Bicycles
                                          .Where(book => book.Serial_number == formattedSerialNumber)
                                          .ToArrayAsync();

                return dtos.Select(Bicycle.Mapper.Map)
                           .ToArray();
            }

            return new Bicycle[0];
        }

        public async Task<Bicycle[]> GetAllByTitleOrProducerAsync(string titleOrProducer)
        {
            var dbContext = dbContextFactory.Create(typeof(BicycleRepository));

            var parameter = new SqlParameter("@titleOrProducer", titleOrProducer);
            var dtos = await dbContext.Bicycles
                                      .FromSqlRaw("SELECT * FROM Books WHERE CONTAINS((Producer, Title), @titleOrProducer)",
                                                  parameter)
                                      .ToArrayAsync();

            return dtos.Select(Bicycle.Mapper.Map)
                       .ToArray();
        }

        public async Task<Bicycle> GetByIdAsync(int id)
        {
            var dbContext = dbContextFactory.Create(typeof(BicycleRepository));

            var dto = await dbContext.Bicycles
                               .SingleAsync(bicycle => bicycle.ID == id);

            return Bicycle.Mapper.Map(dto);
        }
    }
}
