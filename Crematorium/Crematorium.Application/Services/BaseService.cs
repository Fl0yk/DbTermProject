using Crematorium.Application.Abstractions;
using Crematorium.Domain.Abstractions;
using Crematorium.Domain.Entities;
using System.Linq.Expressions;

namespace Crematorium.Application.Services
{
    public abstract class BaseService<T> : IBaseService<T> where T : Base
    {
        protected IRepository<T> _repository;

        public virtual async Task<T> AddAsync(T item)
        {
            await _repository.AddAsync(item);
            return item;
        }

        public virtual async Task<T?> DeleteAsync(int id)
        {
            var item = _repository.FirstOrDefaultAsync(x => x.Id == id).Result;
            if (item != default)
            {
                await _repository.DeleteAsync(item);
            }

            return item;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.ListAllAsync();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public virtual async Task<T> UpdateAsync(T item)
        {
            await _repository.UpdateAsync(item);
            return item;
        }

        public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> filter)
        {
            return await _repository.FirstOrDefaultAsync(filter);
        }

        public virtual async Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> filter)
        {
            return await _repository.ListAsync(filter);
        }
    }
}
