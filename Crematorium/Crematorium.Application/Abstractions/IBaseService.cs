using Crematorium.Domain.Entities;
using System.Linq.Expressions;

namespace Crematorium.Application.Abstractions
{
    public interface IBaseService<T> where T : Base
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> filter);
        Task<T?> GetByIdAsync(int id);
        Task<T> AddAsync(T item);
        Task<T> UpdateAsync(T item);
        Task<T?> DeleteAsync(int id);
        public Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> filter);
    }
}
