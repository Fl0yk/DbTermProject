using Crematorium.Domain.Abstractions;
using Crematorium.Domain.Abstractions.Loggers;
using Crematorium.Persistense.Loggers;
using Crematorium.Persistense.Repository.Postgre;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Crematorium.Persistense.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Lazy<IUserRepository> _userRepository;
        private readonly Lazy<IOrderRepository> _orderRepository;
        private readonly Lazy<ICorposeRepository> _corposeRepository;
        private readonly Lazy<IRitualUrnRepository> _ritualUrnRepository;
        private readonly Lazy<IHallRepository> _hallRepository;
        private readonly Lazy<IUserAuthLogger> _userAuthLogger;

        public UnitOfWork(IConfiguration configuration)
        {
            string connection = "Host=localhost;Username=postgres;Password=moooooo2242003;Database=postgres;ConnectionIdleLifetime=10";

            var source = NpgsqlDataSource.Create(connection);
            //var con = source.OpenConnection();

            _userRepository = new Lazy<IUserRepository>(() =>
                                    new UserRepository(source));
            _orderRepository = new Lazy<IOrderRepository>(() =>
                                    new OrderRepository(source));
            _corposeRepository = new Lazy<ICorposeRepository>(() =>
                                    new CorposeRepository(source));
            _ritualUrnRepository = new Lazy<IRitualUrnRepository>(() =>
                                    new RitualUrnRepository(source));
            _hallRepository = new Lazy<IHallRepository>(() =>
                                    new HallRepository(source));
            _userAuthLogger = new Lazy<IUserAuthLogger>(() => 
                                    new UserAuthLogger(source));
        }

        public IUserRepository UserRepository => _userRepository.Value;

        public IOrderRepository OrderRepository => _orderRepository.Value;

        public ICorposeRepository CorposeRepository => _corposeRepository.Value;

        public IRitualUrnRepository RitualUrnRepository => _ritualUrnRepository.Value;

        public IHallRepository HallRepository => _hallRepository.Value;

        public IUserAuthLogger UserAuthLogger => _userAuthLogger.Value;
    }
}
