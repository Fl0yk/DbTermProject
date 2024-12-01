using Crematorium.Domain.Abstractions;
using Crematorium.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Crematorium.Persistense.Repository.Fake
{
    public class FakeRitualUrnRepository : IRepository<RitualUrn>
    {
        private List<RitualUrn> _ritualUrns;

        public FakeRitualUrnRepository()
        {
            _ritualUrns = new List<RitualUrn>();
            //throw new Exception("Создай урны!");
            using (FileStream fs = new FileStream("urn.json", FileMode.OpenOrCreate))
            {
                RitualUrn? urn = JsonSerializer.Deserialize<RitualUrn>(fs);
                _ritualUrns.Add(urn);
            }
        }
        public Task AddAsync(RitualUrn entity, CancellationToken cancellationToken = default)
        {
            _ritualUrns.Add(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(RitualUrn entity, CancellationToken cancellationToken = default)
        {
            if (_ritualUrns.Contains(entity))
                _ritualUrns.Remove(entity);

            return Task.CompletedTask;
        }

        public Task<RitualUrn?> FirstOrDefaultAsync(Expression<Func<RitualUrn, bool>> filter, CancellationToken cancellationToken = default, params Expression<Func<RitualUrn, object>>[]? includesProperties)
        {
            return Task.FromResult(_ritualUrns.FirstOrDefault(filter.Compile()));
        }

        public Task<RitualUrn?> GetByIdAsync(int id, CancellationToken cancellationToken = default, params Expression<Func<RitualUrn, object>>[]? includesProperties)
        {
            return Task.FromResult(_ritualUrns.FirstOrDefault(u => u.Id == id));
        }

        public Task<IReadOnlyList<RitualUrn>> ListAllAsync(CancellationToken cancellationToken = default, params Expression<Func<RitualUrn, object>>[]? includesProperties)
        {
            return Task.FromResult((IReadOnlyList<RitualUrn>)_ritualUrns.AsReadOnly());
        }

        public Task<IReadOnlyList<RitualUrn>> ListAsync(Expression<Func<RitualUrn, bool>> filter, CancellationToken cancellationToken = default, params Expression<Func<RitualUrn, object>>[]? includesProperties)
        {
            return Task.FromResult((IReadOnlyList<RitualUrn>)_ritualUrns.Where(filter.Compile()).ToList());
        }

        public Task UpdateAsync(RitualUrn entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
