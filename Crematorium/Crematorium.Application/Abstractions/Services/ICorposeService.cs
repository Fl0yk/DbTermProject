using Crematorium.Domain.Entities;

namespace Crematorium.Application.Abstractions.Services;

public interface ICorposeService
{
    public Task<IEnumerable<Corpose>> GetAllAsync(CancellationToken cancellationToken = default);

    public Task<IEnumerable<Corpose>> FindByNameAsync(string name, CancellationToken cancellationToken = default);

    public Task AddAsync(Corpose corpose, CancellationToken cancellationToken = default);

    public Task UpdateAsync(Corpose corpose, CancellationToken cancel = default);

    public Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    public Task<Corpose?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}
