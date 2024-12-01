using Crematorium.Application.Abstractions;
using Crematorium.Domain.Abstractions;
using Crematorium.Domain.Entities;

namespace Crematorium.Application.Services
{
    public class CorposeService : ICorposeService
    {
        private readonly ICorposeRepository _corposeRepository;

        public CorposeService(IUnitOfWork unitOfWork)
        {
            _corposeRepository = unitOfWork.CorposeRepository;
        }

        public async Task AddAsync(Corpose corpose, CancellationToken cancellationToken = default)
        {
            await _corposeRepository.AddAsync(corpose, cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            await _corposeRepository.DeleteAsync(id, cancellationToken);
        }

        public Task<IEnumerable<Corpose>> FindByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return _corposeRepository.FindByNameAsync(name, cancellationToken);
        }

        public Task<IEnumerable<Corpose>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return _corposeRepository.GetAllAsync(cancellationToken);
        }

        public Task<Corpose?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return _corposeRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task UpdateAsync(Corpose corpose, CancellationToken cancel = default)
        {
            await _corposeRepository.UpdateAsync(corpose, cancel);
        }
    }
}
