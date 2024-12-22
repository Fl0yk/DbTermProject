using Crematorium.Domain.Entities;

namespace Crematorium.Application.Abstractions.Services
{
    public interface IOrderService
    {
        public Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken = default);

        public Task<IEnumerable<Order>> GetUserOrdersAsync(User user, CancellationToken cancellationToken = default);

        public Task AddAsync(Order order, CancellationToken cancellationToken = default);

        public Task NextState(Order? order);

        public Task CancelOrder(Order? order);
    }
}
