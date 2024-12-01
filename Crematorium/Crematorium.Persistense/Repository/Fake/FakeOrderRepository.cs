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
    public class FakeOrderRepository : IRepository<Order>
    {
        private List<Order> _orders = new List<Order>();

        public FakeOrderRepository()
        {
            _orders = new List<Order>();
            //throw new Exception("Добавь заказы!");
            using (FileStream fs = new FileStream("order.json", FileMode.OpenOrCreate))
            {
                Order? urn = JsonSerializer.Deserialize<Order>(fs);
                _orders.Add(urn);
            }
        }

        public Task AddAsync(Order entity, CancellationToken cancellationToken = default)
        {
            _orders.Add(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Order entity, CancellationToken cancellationToken = default)
        {
            _orders.Remove(entity);
            return Task.CompletedTask;
        }

        public Task<Order?> FirstOrDefaultAsync(Expression<Func<Order, bool>> filter, CancellationToken cancellationToken = default, params Expression<Func<Order, object>>[]? includesProperties)
        {
            return Task.FromResult(_orders.FirstOrDefault(filter.Compile()));
        }

        public Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken = default, params Expression<Func<Order, object>>[]? includesProperties)
        {
            return Task.FromResult(_orders.FirstOrDefault(u => u.Id == id));
        }

        public Task<IReadOnlyList<Order>> ListAllAsync(CancellationToken cancellationToken = default, params Expression<Func<Order, object>>[]? includesProperties)
        {
            return Task.FromResult((IReadOnlyList<Order>)_orders.AsReadOnly());
        }

        public Task<IReadOnlyList<Order>> ListAsync(Expression<Func<Order, bool>> filter, CancellationToken cancellationToken = default, params Expression<Func<Order, object>>[]? includesProperties)
        {
            return Task.FromResult((IReadOnlyList<Order>)_orders.Where(filter.Compile()).ToList());
        }

        public Task UpdateAsync(Order entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
