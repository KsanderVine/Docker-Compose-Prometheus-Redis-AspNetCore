using System.Text.Json;

namespace UrlSaver.Services
{
    public class DevelopmentCacheService : IDistributedCacheService
    {
        private readonly IDictionary<string, string> _cache = new Dictionary<string, string>();

        public async Task SetRecordAsync(object key, object entity, TimeSpan? expiry = null, int db = -1)
        {
            if (key is null)
                throw new ArgumentNullException(nameof(key));

            if (key.ToString() is null)
                throw new NullReferenceException($"{nameof(key)} can't be null");

            var keyString = key.ToString()!;
            var serialEntity = JsonSerializer.Serialize(entity);

            if (_cache.ContainsKey(keyString))
                _cache[keyString] = serialEntity;
            else
                _cache.Add(keyString, serialEntity);

            await Task.CompletedTask;
        }

        public async Task<TEntity?> GetRecordAsync<TEntity>(object key, int db = -1)
        {
            if (key is null)
                throw new ArgumentNullException(nameof(key));

            if (key.ToString() is null)
                throw new NullReferenceException($"{nameof(key)} can't be null");

            var keyString = key.ToString()!;

            if (_cache.TryGetValue(keyString, out string? serialEntity))
            {
                var obj = JsonSerializer.Deserialize<TEntity>(serialEntity);
                return await Task.FromResult(obj);
            }

            return default;
        }

        public async Task RemoveRecordAsync(object key, int db = -1)
        {
            if (key is null)
                throw new ArgumentNullException(nameof(key));

            if (key.ToString() is null)
                throw new NullReferenceException($"{nameof(key)} can't be null");

            var keyString = key.ToString()!;

            if (_cache.ContainsKey(keyString))
                _cache.Remove(keyString);

            await Task.CompletedTask;
        }
    }
}
