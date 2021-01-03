using System;
using System.Collections.Generic;

namespace Quant.DB.CLI.Models
{
    public partial class Blobs
    {
        public Blobs()
        {
            BlobsMap = new HashSet<BlobsMap>();
        }

        public int BlobsId { get; set; }
        public string FileName { get; set; }
        public byte[] Content { get; set; }

        public virtual ICollection<BlobsMap> BlobsMap { get; set; }
    }
}
