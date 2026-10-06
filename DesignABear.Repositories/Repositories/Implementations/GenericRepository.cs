using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using DesignABear.Repositories.Data;
using DesignABear.Repositories.Repositories.Interfaces;

namespace DesignABear.Repositories.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(object id) => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.Where(predicate).ToListAsync();

        public async Task<IEnumerable<T>> FindAsNoTrackingAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.AsNoTracking().Where(predicate).ToListAsync();

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public async Task AddRangeAsync(IEnumerable<T> entities) => await _dbSet.AddRangeAsync(entities);

        public void Update(T entity) => _dbSet.Update(entity);

        public void Remove(T entity) => _dbSet.Remove(entity);

        public void RemoveRange(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.AnyAsync(predicate);

        public IQueryable<T> GetQueryable() => _dbSet.AsQueryable();

        public void Detach(T entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }
    }
}
