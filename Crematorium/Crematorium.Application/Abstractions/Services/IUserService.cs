using Crematorium.Domain.Entities;

namespace Crematorium.Application.Abstractions.Services
{
    public interface IUserService
    {
        public Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        public Task AddAsync(User user, CancellationToken cancellationToken = default);

        public Task UpdateAsync(User user, CancellationToken cancellationToken = default);

        public Task DeleteAsync(int id, CancellationToken cancellationToken = default);

        public Task<bool> IsExist(string numPassport, CancellationToken cancellationToken = default);

        public Task<bool> IsExist(string name, string numPassport, CancellationToken cancellationToken = default);

        public Task<User?> GetUserByNameAndPassport(string name, string numPassport, CancellationToken cancellationToken = default);

        public Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default);

        public Task<IEnumerable<User>> FindByNameAsync(string name, CancellationToken cancellationToken = default);

    }
}
