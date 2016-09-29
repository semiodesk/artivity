using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Apid.Helpers
{
    public class ProgressInfo
    {
        #region Members

        public Uri Id { get; set; }

        public long Total { get; set; }

        public long Completed { get; set; }

        public float PercentComplete
        {
            get
            {
                return Total > 0 ? 100 * Completed / Total : 100;
            }
        }

        #endregion

        #region Constructors

        public ProgressInfo(Uri id, long total)
        {
            Id = id;
            Total = total;
        }

        #endregion
    }

    public delegate void ProgressChangedEventHandler(object sender, ProgressInfo progressInfo);
}
