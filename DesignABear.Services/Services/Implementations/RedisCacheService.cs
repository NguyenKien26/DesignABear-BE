using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using DesignABear.Services.Services.Interfaces;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Linq;

namespace DesignABear.Services.Services.Implementations
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly IConnectionMultiplexer _redis;
        private readonly ILogger<RedisCacheService> _logger;
        private readonly string _instanceName;
        private readonly string _viewCountKey = "product:views";
        private const string Green = "\x1b[32m";
        private const string Reset = "\x1b[0m";

        private static readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = false
        };

        public RedisCacheService(IDistributedCache cache, IConnectionMultiplexer redis, ILogger<RedisCacheService> logger, IConfiguration configuration)
        {
            _cache = cache;
            _redis = redis;
            _logger = logger;
            _instanceName = configuration["Redis:InstanceName"] ?? "";
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            try
            {
                if (!_redis.IsConnected) return default;

                _logger.LogInformation($"{Green}[Redis] GET: {{Key}}{Reset}", key);
                var data = await _cache.GetStringAsync(key);
                return data == null ? default : JsonSerializer.Deserialize<T>(data, _serializerOptions);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "[Redis] Failed GET for key {Key}", key);
                return default;
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            try
            {
                if (!_redis.IsConnected) return;

                _logger.LogInformation($"{Green}[Redis] SET: {{Key}}{Reset}", key);
                var options = new DistributedCacheEntryOptions();
                if (expiration.HasValue)
                {
                    options.AbsoluteExpirationRelativeToNow = expiration;
                }
                
                var data = JsonSerializer.Serialize(value, _serializerOptions);
                await _cache.SetStringAsync(key, data, options);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "[Redis] Failed SET for key {Key}", key);
            }
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                if (!_redis.IsConnected) return;

                _logger.LogInformation($"{Green}[Redis] REMOVE: {{Key}}{Reset}", key);
                await _cache.RemoveAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "[Redis] Failed REMOVE for key {Key}", key);
            }
        }

        public async Task RemoveByPrefixAsync(string prefix)
        {
            try
            {
                if (!_redis.IsConnected) return;

                string fullPrefix = $"{_instanceName}{prefix}";
                _logger.LogInformation($"{Green}[Redis] REMOVE BY PREFIX: {{FullPrefix}}{Reset}", fullPrefix);
                
                var db = _redis.GetDatabase();
                var endpoints = _redis.GetEndPoints();
                if (endpoints.Length == 0) return;

                var server = _redis.GetServer(endpoints[0]);

                // Efficiently collect keys and delete in batch to minimize network roundtrips
                var keys = new List<RedisKey>();
                foreach (var key in server.Keys(pattern: $"{fullPrefix}*"))
                {
                    keys.Add(key);
                    // Batch size of 1000 to avoid giant allocations or timeouts
                    if (keys.Count >= 1000)
                    {
                        await db.KeyDeleteAsync(keys.ToArray());
                        keys.Clear();
                    }
                }

                if (keys.Any())
                {
                    await db.KeyDeleteAsync(keys.ToArray());
                }
                _logger.LogInformation($"{Green}[Redis] Batch removal complete for prefix '{{FullPrefix}}'{Reset}", fullPrefix);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "[Redis] Failed RemoveByPrefixAsync for prefix {Prefix}", prefix);
            }
        }

        public async Task FlushAllCachedDataAsync()
        {
            try
            {
                if (!_redis.IsConnected) return;

                _logger.LogInformation($"{Green}[Redis] FLUSH: Clearing all cached data (prefix: {{InstanceName}})...{Reset}", _instanceName);

                var db = _redis.GetDatabase();
                var endpoints = _redis.GetEndPoints();
                if (endpoints.Length == 0) return;

                var server = _redis.GetServer(endpoints[0]);

                var keys = new List<RedisKey>();
                foreach (var key in server.Keys(pattern: $"{_instanceName}*"))
                {
                    keys.Add(key);
                    if (keys.Count >= 1000)
                    {
                        await db.KeyDeleteAsync(keys.ToArray());
                        keys.Clear();
                    }
                }

                if (keys.Any())
                {
                    await db.KeyDeleteAsync(keys.ToArray());
                }

                _logger.LogInformation($"{Green}[Redis] FLUSH complete for prefix '{{InstanceName}}'.{Reset}", _instanceName);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "[Redis] Failed FlushAllCachedDataAsync");
            }
        }

        public async Task IncrementViewCountAsync(Guid productId)
        {
            try
            {
                if (!_redis.IsConnected) return;

                var db = _redis.GetDatabase();
                // FireAndForget: User doesn't need to wait for view count update to finish
                await db.HashIncrementAsync(_viewCountKey, productId.ToString(), 1, CommandFlags.FireAndForget);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "[Redis] Failed IncrementViewCountAsync for product {ProductId}", productId);
            }
        }

        public async Task<long> GetViewCountAsync(Guid productId)
        {
            try
            {
                if (!_redis.IsConnected) return 0;

                var db = _redis.GetDatabase();
                var val = await db.HashGetAsync(_viewCountKey, productId.ToString());
                return val.HasValue ? (long)val : 0;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "[Redis] Failed GetViewCountAsync for product {ProductId}", productId);
                return 0;
            }
        }

        public async Task<Dictionary<Guid, long>> GetAllViewCountsAsync()
        {
            var result = new Dictionary<Guid, long>();
            try
            {
                if (!_redis.IsConnected) return result;

                var db = _redis.GetDatabase();

                // HashGetAllAsync is much more efficient than SCAN + multiple GETs
                var entries = await db.HashGetAllAsync(_viewCountKey);
                foreach (var entry in entries)
                {
                    if (Guid.TryParse(entry.Name, out var id))
                    {
                        result[id] = (long)entry.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "[Redis] Failed GetAllViewCountsAsync");
            }
            return result;
        }

        public async Task ClearViewCountAsync(Guid productId)
        {
            try
            {
                if (!_redis.IsConnected) return;

                var db = _redis.GetDatabase();
                await db.HashDeleteAsync(_viewCountKey, productId.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "[Redis] Failed ClearViewCountAsync for product {ProductId}", productId);
            }
        }

        public async Task TrackTrendingCategoryAsync(string categorySlug)
        {
            try
            {
                if (!_redis.IsConnected) return;

                var db = _redis.GetDatabase();
                // FireAndForget: Telemetry shouldn't block user response
                await db.SortedSetIncrementAsync("trending:categories", categorySlug, 1, CommandFlags.FireAndForget);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "[Redis] Failed TrackTrendingCategoryAsync for category {CategorySlug}", categorySlug);
            }
        }
    }
}
