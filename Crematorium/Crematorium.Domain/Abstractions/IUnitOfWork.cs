using Crematorium.Domain.Abstractions.Loggers;

namespace Crematorium.Domain.Abstractions
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IOrderRepository OrderRepository { get; }
        ICorposeRepository CorposeRepository { get; }
        IRitualUrnRepository RitualUrnRepository { get; }
        IHallRepository HallRepository { get; }
        IUserAuthLogger UserAuthLogger { get; }
    }
}
