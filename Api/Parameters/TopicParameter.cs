using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Api.Parameters
{
    public class TopicParameter : Parameter
    {
        #region Members

        public string agent { get; set; }

        public string primarySource { get; set; }

        public DateTime startTime { get; set; }

        public DateTime endTime { get; set; }

        public string title { get; set; }

        public string[] marks { get; set; }

        #endregion

        #region Methods

        public override bool Validate() { return true; }

        #endregion
    }
}
