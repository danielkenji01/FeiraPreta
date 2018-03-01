using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeiraPreta.Domain
{
    public class EventScore
    {
        public Guid Id { get; set; }

        public string Value { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
