using Crematorium.Domain.Entities;

namespace Crematorium.Application.Abstractions.Services;

public interface IRitualUrnService
{
    public Task<IEnumerable<RitualUrn>> GetAllAsync(CancellationToken cancellationToken = default);

    public Task<IEnumerable<RitualUrn>> FindByNameAsync(string name, CancellationToken cancellationToken = default);

    public Task<RitualUrn?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    public Task CreateAsync(RitualUrn urn, CancellationToken cancellationToken = default);

    public Task UpdateAsync(RitualUrn urn, CancellationToken cancellationToken = default);

    public Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
