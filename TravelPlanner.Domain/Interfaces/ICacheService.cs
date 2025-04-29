namespace TravelPlanner.Domain.Interfaces
{
    public interface ICacheService
    {
        Task<string?> GetCompressedJsonAsync(string key);
        Task SetCompressedJsonAsync(string key, string json, TimeSpan ttl);
    }
}
