using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Apid.Helpers
{
    public class ProgressStreamInfo
    {
        #region Members

        public long TotalBytes { get; set; }

        public long TransferredBytes { get; set; }

        public float PercentComplete
        {
            get
            {
                return TotalBytes > 0 ? 100 * TransferredBytes / TotalBytes : 100;
            }
        }

        #endregion

        #region Constructors

        public ProgressStreamInfo(long totalBytes)
        {
            TotalBytes = totalBytes;
        }

        #endregion
    }
}
