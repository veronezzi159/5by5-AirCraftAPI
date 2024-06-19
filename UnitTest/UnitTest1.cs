using _5by5_AirCraftAPI.Data;
using _5by5_AirCraftAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using UnitTest.Services;
namespace UnitTest
{
    public class UnitTest1
    {
        private DbContextOptions<_5by5_AirCraftAPIContext> _options;
        public UnitTest1()
        {
            _options = new DbContextOptionsBuilder<_5by5_AirCraftAPIContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

        }

        [Fact]
        private void PostAirCraftsInMemory()
        {
            using (var context = new _5by5_AirCraftAPIContext(_options))
            {
                for (int i = 0; i < 100; i++)
                {
                    var airCraft = new AirCraft { Rab = "ABC" + i, Capacity = i, DTRegistry = DateTime.Now, DTLastFlight = DateTime.Now, CnpjCompany = "61787912000140" };
                    //AirCraft teste = new CnpjTest().PostAirCraft(airCraft).Result;
                    context.AirCraft.Add(airCraft);
                }
                context.SaveChanges();
                Assert.Equal(100, context.AirCraft.Count());
            }
        }
        private void PostAirCraftsInDatabase()
        {

            for (int i = 0; i < 100; i++)
            {
                var airCraft = new AirCraft { Rab = "ABC" + i, Capacity = i, DTRegistry = DateTime.Now, DTLastFlight = DateTime.Now, CnpjCompany = "61787912000140" };
                AirCraft teste = new CnpjTest().PostAirCraft(airCraft).Result;
            }
        }
    }
}