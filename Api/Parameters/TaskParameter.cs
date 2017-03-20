using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Api.Parameters
{
    public class TaskParameter
    {
        #region Members

        public string uri { get; set; }

        public string agent { get; set; }

        public string entity { get; set; }

        public DateTime startTime { get; set; }

        public DateTime endTime { get; set; }

        public string name { get; set; }

        public bool completed { get; set; }

        public string[] marks { get; set; }

        #endregion
    }
}
