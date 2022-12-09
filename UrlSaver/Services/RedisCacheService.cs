using StackExchange.Redis;
using System.Text.Json;

namespace UrlSaver.Services
{
    public class RedisCacheService : IDistributedCacheService
    {
        private readonly ILogger<RedisCacheService> _logger;
        private readonly IConnectionMultiplexer _cache;

        public RedisCacheService (
            ILogger<RedisCacheService> logger,
            IConnectionMultiplexer cache)
        {
            _logger = logger;
            _cache = cache;
        }

        public async Task SetRecordAsync(object key, object entity, TimeSpan? expiry = null, int db = -1)
        {
            if (!IsConnected())
                return;

            if (key is null)
                throw new ArgumentNullException(nameof(key));

            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            var database = _cache.GetDatabase(db);
            var serialEntity = JsonSerializer.Serialize(entity);

            await database.StringSetAsync(key.ToString(), serialEntity, expiry);
        }

        public async Task<TEntity?> GetRecordAsync<TEntity>(object key, int db = -1)
        {
            if (!IsConnected())
                return default;

            if (key is null)
                throw new ArgumentNullException(nameof(key));

            var database = _cache.GetDatabase(db);

            string? serialEntity = await database.StringGetAsync(key.ToString());

            if (string.IsNullOrWhiteSpace(serialEntity))
                return default;

            var obj = JsonSerializer.Deserialize<TEntity>(serialEntity);
            return obj;
        }

        public async Task RemoveRecordAsync(object key, int db = -1)
        {
            if (!IsConnected())
                return;

            if (key is null)
                throw new ArgumentNullException(nameof(key));

            var database = _cache.GetDatabase(db);

            await database.KeyDeleteAsync(key.ToString());
        }

        private bool IsConnected ()
        {
            if (!_cache.IsConnected)
            {
                _logger.LogWarning("--> Redis not connected");
                return false;
            }

            return true;
        }
    }
}
