using Crematorium.Application.Abstractions.Services;
using Crematorium.Domain.Abstractions;
using Crematorium.Domain.Entities;

namespace Crematorium.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICorposeRepository _corposeRepository;

        public OrderService(IUnitOfWork unitOfWork) 
        {
            _orderRepository = unitOfWork.OrderRepository;
            _corposeRepository = unitOfWork.CorposeRepository;
        }

        public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
        {
            order.CorposeId = (await _corposeRepository.GetAllAsync(cancellationToken)).Where(c => c.NumPassport == order.CorposeId.NumPassport).Last();

            await _orderRepository.CreateAsync(order, cancellationToken);
        }

        public async Task CancelOrder(Order? order)
        {
            if (order is null || order.State == StateOrder.Closed)
                return;

            //order.State = StateOrder.Cancelled;
            await _orderRepository.ChangeStatus(order.Id, StateOrder.Cancelled);

            return;
        }

        public Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return _orderRepository.GetAllAsync(cancellationToken);
        }

        public Task<IEnumerable<Order>> GetUserOrdersAsync(User user, CancellationToken cancellationToken = default)
        {
            return _orderRepository.GetUserOrdersAsync(user, cancellationToken);
        }

        public async Task NextState(Order? order)
        {
            if (order is null)
                return;

            switch (order.State)
            {
                case StateOrder.Decorated:
                    await _orderRepository.ChangeStatus(order.Id, StateOrder.Approved);
                    break;

                case StateOrder.Approved:
                    await _orderRepository.ChangeStatus(order.Id, StateOrder.Closed);
                    break;
            }

            return;
        }
    }
}
