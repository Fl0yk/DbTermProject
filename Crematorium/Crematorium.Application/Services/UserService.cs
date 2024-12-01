using Crematorium.Application.Abstractions;
using Crematorium.Domain.Abstractions;
using Crematorium.Domain.Entities;

namespace Crematorium.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUnitOfWork unitOfWork)
        {
            _userRepository = unitOfWork.UserRepository;
        }

        public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        {
            await _userRepository.AddAsync(user, cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            await _userRepository.DeleteAsync(id, cancellationToken);
        }

        public Task<IEnumerable<User>> FindByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return _userRepository.FindByNameAsync(name, cancellationToken);
        }

        public Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return _userRepository.GetAllAsync(cancellationToken);
        }

        public Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return _userRepository.GetByIdAsync(id, cancellationToken);
        }

        public Task<User?> GetUserByNameAndPassport(string name, string numPassport, CancellationToken cancellationToken = default)
        {
            return _userRepository.GetUserByNameAndPassport(name, numPassport, cancellationToken);
        }

        public Task<bool> IsExist(string numPassport, CancellationToken cancellationToken = default)
        {
            return _userRepository.IsExist(numPassport, cancellationToken);
        }

        public Task<bool> IsExist(string name, string numPassport, CancellationToken cancellationToken = default)
        {
            return _userRepository.IsExist(name, numPassport, cancellationToken);
        }

        public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            await _userRepository.UpdateAsync(user, cancellationToken);
        }
    }
}
