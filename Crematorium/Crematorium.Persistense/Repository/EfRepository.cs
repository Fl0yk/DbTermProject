using Crematorium.Domain.Abstractions;
using Crematorium.Domain.Entities;
using Crematorium.Persistense.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Crematorium.Persistense.Repository
{
    public class EfRepository<T> : IRepository<T> where T : Base
    {
        protected readonly CrematoriumDbContext _context;
        protected readonly DbSet<T> _entities;

        public EfRepository(CrematoriumDbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public async Task AddAsync(T entity, 
            CancellationToken cancellationToken = default)
        {
            await _entities.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync();
            return;
        }

        public Task DeleteAsync(T entity, 
            CancellationToken cancellationToken = default)
        {
            if(_entities.Contains(entity))
                    _entities.Remove(entity);

            _context.SaveChanges();
            return Task.CompletedTask;
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> filter, 
            CancellationToken cancellationToken = default, params Expression<Func<T, object>>[]? includesProperties)
        {
            IQueryable<T>? query = _entities.AsQueryable();
            if (includesProperties.Any())
            {
                foreach (Expression<Func<T, object>>? included in includesProperties)
                {
                    query = query.Include(included);
                }
            }
            try
            {
                var result = await query.FirstAsync(filter, cancellationToken);

                return result;
            }
            catch (InvalidOperationException)
            {
                return default;
            }
        }

        public async Task<T?> GetByIdAsync(int id, 
            CancellationToken cancellationToken = default, 
            params Expression<Func<T, object>>[]? includesProperties)
        {
            return await FirstOrDefaultAsync(e => e.Id == id, CancellationToken.None,  includesProperties);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default,
                                                    params Expression<Func<T, object>>[]? includesProperties)
        {
            IQueryable<T>? query = _entities.AsQueryable();

            if (includesProperties.Any())
            {
                foreach (Expression<Func<T, object>>? included in includesProperties)
                {
                    query = query.Include(included);
                }
            }

            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> filter, 
            CancellationToken cancellationToken = default, 
            params Expression<Func<T, object>>[]? includesProperties)
        {
            IQueryable<T>? query = _entities.AsQueryable();

            if (includesProperties.Any())
            {
                foreach (Expression<Func<T, object>>? included in
               includesProperties)
                {
                    query = query.Include(included);
                }
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }

        public Task UpdateAsync(T entity, 
            CancellationToken cancellationToken = default)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
