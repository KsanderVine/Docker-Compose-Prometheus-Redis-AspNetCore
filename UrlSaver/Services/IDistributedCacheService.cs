namespace UrlSaver.Services
{
    public interface IDistributedCacheService
    {
        Task SetRecordAsync(object key, object entity, TimeSpan? expiry = null, int db = -1);
        Task<TEntity?> GetRecordAsync<TEntity>(object key, int db = -1);
        Task RemoveRecordAsync(object key, int db = -1);
    }
}
