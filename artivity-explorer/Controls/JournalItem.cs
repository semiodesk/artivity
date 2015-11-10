using System;
using Semiodesk.Trinity;

namespace Artivity.Explorer
{
    public class JournalItem
    {
        public Uri Agent { get; set; }

        public string Url { get; set; }

        public string Path { get; set; }

        public string FileName
        {
            get { return System.IO.Path.GetFileName(Path); }
        }

        [NotifyPropertyChanged]
        public DateTime LastEditingDate { get; set; }

        public string FormattedLastEditingDate
        {
            get { return " " + LastEditingDate.ToLocalTime().ToString("t"); }
        }

        [NotifyPropertyChanged]
        public TimeSpan TotalEditingTime { get; set; }

        public string FormattedTotalEditingTime
        {
            get { return TotalEditingTime.ToString("g"); }
        }
    }
}

