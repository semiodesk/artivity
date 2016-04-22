using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artivity.DataModel.Journal
{
    public class Activity
    {
        public Uri AgentUri { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
