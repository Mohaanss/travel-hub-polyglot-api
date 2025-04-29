using StackExchange.Redis;
using System.Text.Json;
using TravelPlanner.Domain.Interfaces;

namespace TravelPlanner.Infrastructure.Redis
{
    public class RedisNotificationService : INotificationService
    {
        private readonly ISubscriber _subscriber;

        public RedisNotificationService(IConnectionMultiplexer redis)
        {
            _subscriber = redis.GetSubscriber();
        }

        public async Task PublishOfferCreatedAsync(string offerId, string from, string to)
        {
            var payload = JsonSerializer.Serialize(new
            {
                offerId,
                from,
                to
            });

            await _subscriber.PublishAsync("offers:new", payload);
        }
    }
}
