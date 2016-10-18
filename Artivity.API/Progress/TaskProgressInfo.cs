using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Apid.Helpers
{
    public class TaskProgressInfo
    {
        #region Members

        public Uri Id { get; set; }

        public long Total { get; set; }

        private long _completed;

        public long Completed
        {
            get { return _completed; }
            set
            {
                _completed = value;

                PercentComplete = Total > 0 ? 100 * value / Total : 100;
            }
        }

        public float PercentComplete { get; private set; }

        #endregion

        #region Constructors

        public TaskProgressInfo(Uri id = null, long total = 0)
        {
            Id = id;
            Total = total;
        }

        #endregion

        #region Methods

        public void Reset()
        {
            Completed = 0;
        }

        #endregion
    }

    public delegate void ProgressChangedEventHandler(object sender, TaskProgressInfo progressInfo);
}
