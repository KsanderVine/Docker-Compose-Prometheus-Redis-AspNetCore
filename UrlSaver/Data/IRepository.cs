using System.Linq.Expressions;

namespace UrlSaver.Data
{
    public interface IRepository<TEntity> : IDisposable
    {
        IEnumerable<TEntity?>? GetAll();
        IEnumerable<TEntity> GetWhere(Expression<Func<TEntity, bool>> predicate);

        Task CreateAsync(TEntity entity);
        void Create(TEntity entity);

        Task<bool> UpdateAsync(TEntity entity);
        bool Update(TEntity entity);

        Task<bool> SaveAsync();
        bool Save();

        void Delete(TEntity entity);
    }

    public interface IRepository<TKey, TEntity> : IRepository<TEntity>, IDisposable
    {
        TEntity? GetByKey(TKey key);
    }
}
