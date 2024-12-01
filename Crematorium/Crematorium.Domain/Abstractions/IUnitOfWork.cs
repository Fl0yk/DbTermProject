using Crematorium.Domain.Entities;

namespace Crematorium.Domain.Abstractions
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IOrderRepository OrderRepository { get; }
        ICorposeRepository CorposeRepository { get; }
        IRitualUrnRepository RitualUrnRepository { get; }
        IHallRepository HallRepository { get; }
        public Task RemoveDatbaseAsync();
        public Task CreateDatabaseAsync();
        public Task SaveAllAsync();
    }
}
