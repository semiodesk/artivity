using System;
using Semiodesk.Trinity;

namespace Artivity.Explorer
{
    public class JournalViewListItem
    {
        public Uri Agent { get; set; }

        // TODO: Map into a URI.
        public string FileUrl { get; set; }

        public string FilePath { get; set; }

        public string FileName
        {
            get { return System.IO.Path.GetFileName(FilePath); }
        }

        [NotifyPropertyChanged]
        public DateTime LastEditingDate { get; set; }

        public string FormattedLastEditingDate
        {
            get { return LastEditingDate.ToLocalTime().ToString("d MMM\nh:m"); }
        }

        [NotifyPropertyChanged]
        public TimeSpan TotalEditingTime { get; set; }

        public string FormattedTotalEditingTime
        {
            get { return TotalEditingTime.ToString("g"); }
        }
    }
}

