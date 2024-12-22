using Crematorium.Application.Abstractions.Services;
using Crematorium.Domain.Entities;

namespace Crematorium.Application.Services
{
    public class HelpersService<T> : BaseService<T>, IHelpersService<T> where T : Entity
    {
        public async Task<IEnumerable<T>> FindByName(string name)
        {
            return await _repository.ListAsync(u => u.Name == name);
        }
    }
}
