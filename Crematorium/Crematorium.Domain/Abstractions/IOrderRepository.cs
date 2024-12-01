using Crematorium.Domain.Entities;

namespace Crematorium.Domain.Abstractions;

public interface IOrderRepository
{
    public Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken = default);

    public Task<IEnumerable<Order>> GetUserOrdersAsync(User user, CancellationToken cancellationToken = default);

    public Task CreateAsync(Order order, CancellationToken cancellationToken = default);

    public Task ChangeStatus(int orderId, StateOrder newState, CancellationToken cancellationToken = default);
}
