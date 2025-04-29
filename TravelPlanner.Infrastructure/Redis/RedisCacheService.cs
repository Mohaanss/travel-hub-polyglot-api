using StackExchange.Redis;
using TravelPlanner.Domain.Interfaces;
using System.IO.Compression;
using System.Text;

namespace TravelPlanner.Infrastructure.Redis
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _db;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task<string?> GetCompressedJsonAsync(string key)
        {
            var value = await _db.StringGetAsync(key);
            if (value.IsNullOrEmpty) return null;

            var compressedBytes = (byte[])value!;
            using var input = new MemoryStream(compressedBytes);
            using var gzip = new GZipStream(input, CompressionMode.Decompress);
            using var reader = new StreamReader(gzip);
            return await reader.ReadToEndAsync();
        }

        public async Task SetCompressedJsonAsync(string key, string json, TimeSpan ttl)
        {
            using var output = new MemoryStream();
            using (var gzip = new GZipStream(output, CompressionMode.Compress))
            using (var writer = new StreamWriter(gzip))
            {
                await writer.WriteAsync(json);
            }

            await _db.StringSetAsync(key, output.ToArray(), ttl);
        }
    }
}
