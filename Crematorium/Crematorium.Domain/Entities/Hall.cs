using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crematorium.Domain.Entities
{
    public class Hall : Entity
    {
        public int Capacity { get; set; }

        public decimal Price { get; set; }

        public List<Date> FreeDates { get; set; } = new();
    }
}
