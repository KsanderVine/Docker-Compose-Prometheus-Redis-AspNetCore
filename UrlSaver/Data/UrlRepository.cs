using System.Linq.Expressions;
using UrlSaver.Models;

namespace UrlSaver.Data
{
    public class UrlRepository : IUrlRepository
    {
        private readonly AppDbContext _context;

        public UrlRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Create(Url entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            _context.Urls.Add(entity);
        }

        public async Task CreateAsync(Url entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            await _context.Urls.AddAsync(entity);
        }

        public void Delete(Url entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            _context.Urls.Remove(entity);
        }

        public IEnumerable<Url?>? GetAll()
        {
            return _context.Urls.ToList();
        }

        public Url? GetByKey(Guid key)
        {
            return _context.Urls.FirstOrDefault(e => e.Id == key);
        }

        public IEnumerable<Url> GetWhere(Expression<Func<Url, bool>> predicate)
        {
            return _context.Urls.Where(predicate);
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool Update(Url entity)
        {
            _context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> UpdateAsync(Url entity)
        {
            _context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
