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

        public string Subtitle { get; set; }

        public DateTime CreatedDateInstagram { get; set; }

        public string ImageLowResolution { get; set; }

        public string ImageThumbnail { get; set; }

        public string ImageStandardResolution { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool IsHighlight { get; set; }

        #region Navigation

        public Person Person { get; set; }

        #endregion
    }
}
