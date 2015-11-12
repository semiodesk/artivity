using System;
using Semiodesk.Trinity;

namespace Artivity.Explorer.Controls
{
    public class ActivityLogItem
    {
        #region Members

        public UriRef Activity { get; set; }

        public UriRef Agent { get;  set; }

        public string AgentColour { get; set; }

        public DateTime Date { get; set; }

        public string FormattedTime
        {
            get { return Date.ToString("HH:mm:ss"); }
        }

        public string InfluenceType { get;  set; }

        public string InfluencedRegion { get; set; }

        public string Description { get; set; }

        public string Data { get; set; }

        #endregion
    }
}

