using System;

namespace ArtivityExplorer
{
	public static class ColorExtensions
	{
		public static System.Drawing.Color ToSystemColor(this Xwt.Drawing.Color c)
		{
			int a = Convert.ToInt32(c.Alpha * byte.MaxValue);
			int r = Convert.ToInt32(c.Red * byte.MaxValue);
			int g = Convert.ToInt32(c.Green * byte.MaxValue);
			int b = Convert.ToInt32(c.Blue * byte.MaxValue);

			return System.Drawing.Color.FromArgb(a, r, g, b);
		}

		public static Xwt.Drawing.Color ToXwtColor(this System.Drawing.Color c)
		{
			double r = c.R > 0 ? (double)c.R / byte.MaxValue : 0;
			double g = c.G > 0 ? (double)c.G / byte.MaxValue : 0;
			double b = c.G > 0 ? (double)c.B / byte.MaxValue : 0;
			double a = c.A > 0 ? (double)c.A / byte.MaxValue : 0;

			return new Xwt.Drawing.Color(r, g, b, a);
		}
	}
}

