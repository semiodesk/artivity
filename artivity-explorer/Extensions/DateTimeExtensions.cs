using System;

namespace Artivity.Explorer
{
	public static class DateTimeExtensions
	{
		public static DateTime RoundToMinute(this DateTime time)
		{
			return new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, 0, time.Kind);
		}

        public static DateTime RoundToDay(this DateTime time)
        {
            return new DateTime(time.Year, time.Month, time.Day, 0, 0, 0, time.Kind);
        }
	}
}

