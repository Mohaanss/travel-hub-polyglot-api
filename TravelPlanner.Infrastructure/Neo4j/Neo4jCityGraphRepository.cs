using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Neo4j.Driver;
using TravelPlanner.Domain.Interfaces;

namespace TravelPlanner.Infrastructure.Neo4j
{
    public class Neo4jCityGraphRepository : ICityGraphRepository
    {
        private readonly IDriver _driver;

        public Neo4jCityGraphRepository(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<List<string>> GetRelatedOffersAsync(string cityCode, DateTime departDate, string excludeOfferId, int k = 3)
        {
            var session = _driver.AsyncSession();

            try
            {
                var query = @"
                    MATCH (fromCity:City {code: $cityCode})
                    MATCH (fromCity)-[:NEAR]->(near:City)
                    MATCH (related:Offer)-[:FROM]->(near)
                    WHERE abs(duration.inDays(date($departDate), related.departDate).days) <= 1
                      AND related.offerId <> $excludeOfferId
                    RETURN related.offerId AS id
                    LIMIT $k";

                var result = await session.ExecuteReadAsync(async tx =>
                {
                    var cursor = await tx.RunAsync(query, new
                    {
                        cityCode,
                        departDate = departDate.ToString("yyyy-MM-dd"),
                        excludeOfferId,
                        k
                    });

                    return await cursor.ToListAsync(r => r["id"].As<string>());
                });

                return result;
            }
            finally
            {
                await session.CloseAsync();
            }
        }
    }
}
