using System;
using System.Collections.Generic;

namespace Quant.DB.CLI.Models
{
    public partial class BlobsMap
    {
        public int BlobsMapId { get; set; }
        public int BlobsIdRef { get; set; }
        public long CorrelationIdRef { get; set; }
        public string MapType { get; set; }

        public virtual Blobs BlobsIdRefNavigation { get; set; }
    }
}
