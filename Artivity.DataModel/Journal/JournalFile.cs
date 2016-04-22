using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artivity.DataModel.Journal
{
    public class JournalFile
    {
        public Uri Agent { get; set; }

        // TODO: Map into a URI.
        public string FileUrl { get; set; }

        public string FilePath { get; set; }

        public string FileName
        {
            get { return System.IO.Path.GetFileName(FilePath); }
        }

        public DateTime LastEditingDate { get; set; }

        public string FormattedLastEditingDate
        {
            get { return " " + LastEditingDate.ToLocalTime().ToString("ddd, hh:mm"); }
        }

        public TimeSpan TotalEditingTime { get; set; }

        public string FormattedTotalEditingTime
        {
            get { return TotalEditingTime.ToString("g"); }
        }
    }
}
