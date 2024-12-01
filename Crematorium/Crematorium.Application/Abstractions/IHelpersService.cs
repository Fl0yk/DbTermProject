using Crematorium.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crematorium.Application.Abstractions
{
    public interface IHelpersService<T> : IBaseService<T> where T : Entity
    {
        public Task<IEnumerable<T>> FindByName(string name);
    }
}
