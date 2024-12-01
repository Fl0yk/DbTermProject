using Crematorium.Domain.Abstractions;
using Crematorium.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Crematorium.Persistense.Repository.Fake
{
    public class FakeCorposeRepository : IRepository<Corpose>
    {
        private List<Corpose> _Corposes;

        public FakeCorposeRepository()
        {
            _Corposes = new List<Corpose>();
            //throw new Exception("Создай урны!");
        }
        public Task AddAsync(Corpose entity, CancellationToken cancellationToken = default)
        {
            _Corposes.Add(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Corpose entity, CancellationToken cancellationToken = default)
        {
            if (_Corposes.Contains(entity))
                _Corposes.Remove(entity);

            return Task.CompletedTask;
        }

        public Task<Corpose?> FirstOrDefaultAsync(Expression<Func<Corpose, bool>> filter, CancellationToken cancellationToken = default, params Expression<Func<Corpose, object>>[]? includesProperties)
        {
            return Task.FromResult(_Corposes.FirstOrDefault(filter.Compile()));
        }

        public Task<Corpose?> GetByIdAsync(int id, CancellationToken cancellationToken = default, params Expression<Func<Corpose, object>>[]? includesProperties)
        {
            return Task.FromResult(_Corposes.FirstOrDefault(u => u.Id == id));
        }

        public Task<IReadOnlyList<Corpose>> ListAllAsync(CancellationToken cancellationToken = default, params Expression<Func<Corpose, object>>[]? includesProperties)
        {
            return Task.FromResult((IReadOnlyList<Corpose>)_Corposes.AsReadOnly());
        }

        public Task<IReadOnlyList<Corpose>> ListAsync(Expression<Func<Corpose, bool>> filter, CancellationToken cancellationToken = default, params Expression<Func<Corpose, object>>[]? includesProperties)
        {
            return Task.FromResult((IReadOnlyList<Corpose>)_Corposes.Where(filter.Compile()).ToList());
        }

        public Task UpdateAsync(Corpose entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
