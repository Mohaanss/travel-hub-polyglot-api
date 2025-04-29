using StackExchange.Redis;
using TravelPlanner.Domain.Interfaces;

namespace TravelPlanner.Infrastructure.Redis
{
    public class RedisService : IRedisService
    {
        private readonly IDatabase _database;

        public RedisService(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }

        public async Task SetSessionAsync(string key, string value, TimeSpan expiration)
        {
            await _database.StringSetAsync(key, value, expiration);
        }
    }
}
