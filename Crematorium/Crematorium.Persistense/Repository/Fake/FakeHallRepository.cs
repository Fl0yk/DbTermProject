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
    public class FakeHallRepository : IRepository<Hall>
    {
        private List<Hall> _halls;

        public FakeHallRepository()
        {
            _halls = new List<Hall>
            {
                new Hall() { Capacity = 30, Id = 1, Name = "Hall 1", Price = 280, FreeDates = new() { new Date() { Id = 1, Data = "16.05.2023" }, new Date() { Id = 2, Data = "18.05.2023" } } },
                new Hall() { Capacity = 12, Id = 2, Name = "Hall 2", Price = 200, FreeDates = new() { new Date() { Id = 3, Data = "17.05.2023" }, new Date() { Id = 4, Data = "20.05.2023" } } }
            };
            //throw new Exception("Создай залы!");
        }

        public Task AddAsync(Hall entity, CancellationToken cancellationToken = default)
        {
            _halls.Add(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Hall entity, CancellationToken cancellationToken = default)
        {
            if (_halls.Contains(entity))
                _halls.Remove(entity);

            return Task.CompletedTask;
        }

        public Task<Hall?> FirstOrDefaultAsync(Expression<Func<Hall, bool>> filter, CancellationToken cancellationToken = default, params Expression<Func<Hall, object>>[]? includesProperties)
        {
            return Task.FromResult(_halls.FirstOrDefault(filter.Compile()));
        }

        public Task<Hall?> GetByIdAsync(int id, CancellationToken cancellationToken = default, params Expression<Func<Hall, object>>[]? includesProperties)
        {
            return Task.FromResult(_halls.FirstOrDefault(u => u.Id == id));
        }

        public Task<IReadOnlyList<Hall>> ListAllAsync(CancellationToken cancellationToken = default, params Expression<Func<Hall, object>>[]? includesProperties)
        {
            return Task.FromResult((IReadOnlyList<Hall>)_halls.AsReadOnly());
        }

        public Task<IReadOnlyList<Hall>> ListAsync(Expression<Func<Hall, bool>> filter, CancellationToken cancellationToken = default, params Expression<Func<Hall, object>>[]? includesProperties)
        {
            return Task.FromResult((IReadOnlyList<Hall>)_halls.Where(filter.Compile()).ToList());
        }

        public Task UpdateAsync(Hall entity, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
