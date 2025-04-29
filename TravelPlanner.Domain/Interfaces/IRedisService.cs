namespace TravelPlanner.Domain.Interfaces
{
    public interface IRedisService
    {
        Task SetSessionAsync(string key, string value, TimeSpan expiration);
    }
}
