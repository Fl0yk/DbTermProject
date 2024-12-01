//using Crematorium.Domain.Abstractions;
//using Crematorium.Domain.Entities;
//using Crematorium.Persistense.Data;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Crematorium.Persistense.Repository.Fake
//{
//    public class FakeUnitOfWork : IUnitOfWork
//    {
//        private readonly Lazy<IRepository<User>> _userRepository;
//        private readonly Lazy<IRepository<Order>> _orderRepository;
//        private readonly Lazy<IRepository<Corpose>> _corposeRepository;
//        private readonly Lazy<IRepository<RitualUrn>> _ritualUrnRepository;
//        private readonly Lazy<IRepository<Hall>> _hallRepository;

//        public FakeUnitOfWork()
//        {
//            _userRepository = new Lazy<IRepository<User>>(() =>
//                                    new FakeUsersRepository());
//            _orderRepository = new Lazy<IRepository<Order>>(() =>
//                                    new FakeOrderRepository());
//            _corposeRepository = new Lazy<IRepository<Corpose>>(() =>
//                                    new FakeCorposeRepository());
//            _hallRepository = new Lazy<IRepository<Hall>>(() =>
//                                    new FakeHallRepository());
//            _ritualUrnRepository = new Lazy<IRepository<RitualUrn>>(() =>
//                                    new FakeRitualUrnRepository());
//        }
//        public IRepository<User> UserRepository => _userRepository.Value;

//        public IRepository<Order> OrderRepository => _orderRepository.Value;

//        public IRepository<Corpose> CorposeRepository => _corposeRepository.Value;

//        public IRepository<RitualUrn> RitualUrnRepository => _ritualUrnRepository.Value;

//        public IRepository<Hall> HallRepository => _hallRepository.Value;

//        public Task CreateDatabaseAsync()
//        {
//            throw new NotImplementedException();
//        }

//        public Task RemoveDatbaseAsync()
//        {
//            throw new NotImplementedException();
//        }

//        public Task SaveAllAsync()
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
