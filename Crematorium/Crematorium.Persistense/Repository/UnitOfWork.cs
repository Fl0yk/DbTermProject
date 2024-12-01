using Crematorium.Domain.Abstractions;
using Crematorium.Domain.Entities;
using Crematorium.Persistense.Data;
using Crematorium.Persistense.Repository.Postgre;
using Microsoft.Extensions.Configuration;

namespace Crematorium.Persistense.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CrematoriumDbContext _context;

        private readonly Lazy<IUserRepository> _userRepository;
        private readonly Lazy<IOrderRepository> _orderRepository;
        private readonly Lazy<ICorposeRepository> _corposeRepository;
        private readonly Lazy<IRitualUrnRepository> _ritualUrnRepository;
        private readonly Lazy<IHallRepository> _hallRepository;

        public UnitOfWork(CrematoriumDbContext dbContext, IConfiguration configuration)
        {
            string connection = "Host=localhost;Username=postgres;Password=moooooo2242003;Database=postgres"; //configuration["postgre"]
                                                //?? throw new KeyNotFoundException("No connection string");

            _context = dbContext;
            _userRepository = new Lazy<IUserRepository>(() =>
                                    new UserRepository(connection));
            _orderRepository = new Lazy<IOrderRepository>(() =>
                                    new OrderRepository(connection));
            _corposeRepository = new Lazy<ICorposeRepository>(() =>
                                    new CorposeRepository(connection));
            _ritualUrnRepository = new Lazy<IRitualUrnRepository>(() =>
                                    new RitualUrnRepository(connection));
            _hallRepository = new Lazy<IHallRepository>(() =>
                                    new HallRepository(connection));
        }

        public IUserRepository UserRepository => _userRepository.Value;

        public IOrderRepository OrderRepository => _orderRepository.Value;

        public ICorposeRepository CorposeRepository => _corposeRepository.Value;

        public IRitualUrnRepository RitualUrnRepository => _ritualUrnRepository.Value;

        public IHallRepository HallRepository => _hallRepository.Value;

        public async Task CreateDatabaseAsync()
        {
            await _context.Database.EnsureCreatedAsync();
        }

        public async Task RemoveDatbaseAsync()
        {
            await _context.Database.EnsureDeletedAsync();
        }

        public async Task SaveAllAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
