using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Apid.Helpers
{
    public class TaskProgressInfoList
    {
        #region Members

        private readonly List<TaskProgressInfo> _tasks = new List<TaskProgressInfo>();

        public IList<TaskProgressInfo> Tasks
        {
            get { return _tasks; }
        }

        public TaskProgressInfo CurrentTask { get; set; }

        public float PercentComplete
        {
            get
            {
                float result = 0;

                foreach (TaskProgressInfo progress in _tasks)
                {
                    result += progress.PercentComplete;
                }

                return Math.Min(100, (result / _tasks.Count));
            }
        }

        #endregion

        #region Methods

        public void Reset()
        {
            foreach(TaskProgressInfo task in _tasks)
            {
                task.Reset();
            }
        }

        #endregion
    }
}
