using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeiraPreta.Domain
{
    public class Highlight
    {
        public Guid Id { get; set; }

        public Guid publicationId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        #region Navigation

        public Publication Publication { get; set; }

        #endregion
    }
}
