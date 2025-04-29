using Neo4j.Driver;
using TravelPlanner.Domain.Interfaces;

namespace TravelPlanner.Infrastructure.Neo4j
{
    public class Neo4jRecommendationRepository : IRecommendationRepository
    {
        private readonly IDriver _driver;

        public Neo4jRecommendationRepository(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<IEnumerable<string>> GetRecommendationsAsync(string city, int k)
        {
            var recommendations = new List<string>();

            var session = _driver.AsyncSession();

            try
            {
                var result = await session.RunAsync(@"
                    MATCH (c:City {code: $city})-[:NEAR]->(n:City)
                    RETURN n.code AS city
                    ORDER BY n.weight DESC
                    LIMIT $k
                ", new { city, k });

                await result.ForEachAsync(record =>
                {
                    var cityCode = record["city"].As<string>();
                    recommendations.Add(cityCode);
                });
            }
            finally
            {
                await session.CloseAsync();
            }

            return recommendations;
        }
    }
}
