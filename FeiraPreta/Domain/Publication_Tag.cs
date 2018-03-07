using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeiraPreta.Domain
{
    public class Publication_Tag
    {
        public Guid PublicationId { get; set; }

        public Guid TagId { get; set; }

        #region navigation

        public Publication Publication { get; set; }

        public Tag Tag { get; set; }

        #endregion
    }
}
