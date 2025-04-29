using Neo4j.Driver;
using TravelPlanner.Domain.Interfaces;

namespace TravelPlanner.Infrastructure.Neo4j
{
    public class Neo4jCityGraphService : ICityGraphService
    {
        private readonly IDriver _driver;

        public Neo4jCityGraphService(IDriver driver)
        {
            _driver = driver;
        }

        public async Task EnsureCitiesAndRelationAsync(string fromCityCode, string toCityCode, double weight)
        {
            var session = _driver.AsyncSession();

            try
            {
                await session.WriteTransactionAsync(async tx =>
                {
                    await tx.RunAsync(@"
                        MERGE (from:City {code: $from})
                        MERGE (to:City {code: $to})
                        MERGE (from)-[r:NEAR]->(to)
                        SET r.weight = $weight
                    ", new { from = fromCityCode, to = toCityCode, weight });
                });
            }
            finally
            {
                await session.CloseAsync();
            }
        }
    }
}
