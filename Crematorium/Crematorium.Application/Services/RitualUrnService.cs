using Crematorium.Application.Abstractions.Services;
using Crematorium.Domain.Abstractions;
using Crematorium.Domain.Entities;

namespace Crematorium.Application.Services
{
    public class RitualUrnService : IRitualUrnService
    {
        private readonly IRitualUrnRepository _urnRepository;

       public RitualUrnService(IUnitOfWork unitOfWork) 
       {
            _urnRepository = unitOfWork.RitualUrnRepository;
       }

        public async Task CreateAsync(RitualUrn urn, CancellationToken cancellationToken = default)
        {
            await _urnRepository.CreateAsync(urn, cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            await _urnRepository.DeleteAsync(id, cancellationToken);
        }

        public Task<IEnumerable<RitualUrn>> FindByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return _urnRepository.FindByNameAsync(name, cancellationToken);
        }

        public Task<IEnumerable<RitualUrn>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return _urnRepository.GetAllAsync(cancellationToken);
        }

        public Task<RitualUrn?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return _urnRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task UpdateAsync(RitualUrn urn, CancellationToken cancellationToken = default)
        {
            await _urnRepository.UpdateAsync(urn, cancellationToken);
        }
    }
}
