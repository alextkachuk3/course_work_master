using StackExchange.Redis;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace MathService.ApiService.Services
{
    public class RedisCacheService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _db;
        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _db = redis.GetDatabase();
        }

        public async Task<string> GetCachedResultAsync(string key)
        {
            return await _db.StringGetAsync(key);
        }

        public async Task CacheResultAsync(string key, object value)
        {
            var jsonData = JsonSerializer.Serialize(value);
            await _db.StringSetAsync(key, jsonData, TimeSpan.FromMinutes(10));
        }
    }
}
