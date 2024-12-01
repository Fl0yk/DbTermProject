using Crematorium.Application.Abstractions;
using Crematorium.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
