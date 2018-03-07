using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeiraPreta.Domain
{
    public class Tag
    {
        public Guid Id { get; set; }

        public string Nome { get; set; }

        #region Navigation

        public ICollection<Domain.Publication_Tag> Publication_Tags { get; set; }

        #endregion
    }
}
