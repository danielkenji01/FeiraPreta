using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeiraPreta.Domain
{
    public class Publication
    {
        public Guid Id { get; set; }

        public Guid PersonId { get; set; }

        public string Link { get; set; }

        public DateTime CreatedDateInstagram { get; set; }

        public Byte[] ImageLowResolution { get; set; }

        public Byte[] ImageThumbnail { get; set; }

        public Byte[] ImageStandardResolution { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        #region Navigation

        public Person Person { get; set; }

        #endregion
    }
}
