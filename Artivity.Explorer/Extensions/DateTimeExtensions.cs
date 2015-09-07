using System;

namespace ArtivityExplorer
{
	public static class DateTimeExtensions
	{
		public static DateTime RoundToMinute(this DateTime time)
		{
			return new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, 0, time.Kind);
		}
	}
}

