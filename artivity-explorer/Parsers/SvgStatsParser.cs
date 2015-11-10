using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Eto.Drawing;

namespace Artivity.Explorer.Parsers
{
    public class SvgStatsParser
    {
        #region Methods

        public static SvgStats TryParse(string filename)
        {
            if (!File.Exists(filename)) return null;

            try
            {
                SvgStats stats = new SvgStats();

    			XmlDocument document = new XmlDocument();
    			document.Load(filename);

    			HashSet<string> ignore = new HashSet<string>() { "svg", "metadata", "namedview" };

    			foreach (XmlElement e in document.SelectNodes("//*"))
    			{
    				if (ignore.Contains(e.LocalName)) continue;

    				TryParseElement(stats, e);
    			}

                stats.SortColours();

                return stats;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return null;
            }
        }

		private static void TryParseElement(SvgStats stats, XmlElement e)
        {
            stats.ElementCount += 1;

			if(e.Name == "g")
            {
				if (e.HasAttribute("inkscape:groupmode") && e.GetAttribute("inkscape:groupmode").ToLowerInvariant() == "layer")
				{
					stats.LayerCount += 1;
				}
				else
				{
					stats.GroupCount += 1;
				}
            }
			else if (e.HasAttribute("clip-path"))
            {
                stats.ClipCount += 1;
            }
			else if(e.HasAttribute("mask") && e.GetAttribute("mask").ToLowerInvariant() != "none")
            {
                stats.MaskCount += 1;
            }
				
			TryParseElementColour(stats, e);
        }

		private static void TryParseElementColour(SvgStats stats, XmlElement e)
        {
			// Parse colours which are direct attributes of the XML element.
			if (e.HasAttribute("fill"))
			{
                Color c = Color.Parse(e.GetAttribute("fill"));

				stats.AddColour(c);
			}

			if (e.HasAttribute("stroke"))
			{
                Color c = Color.Parse(e.GetAttribute("stroke"));

				stats.AddColour(c);
			}

			if (e.HasAttribute("stop-color"))
			{
                Color c = Color.Parse(e.GetAttribute("stop-color"));

				stats.AddColour(c);
			}

			// Parse colours which are part of a style attribute.
			if (!e.HasAttribute("style")) return;

			string style = e.GetAttribute("style").TrimEnd(';');

			foreach (string attribute in style.Split(';'))
			{
				string[] x = attribute.Split(':');

				if (x.Length < 2) continue;

				string key = x[0];
				string value = x[1];

				switch (key)
				{
					case "fill":
					case "stroke":
					case "stop-color":
					{
						Color c = Color.Parse(value);

						stats.AddColour(c);

						break;
					}
				}
			}
        }

        #endregion
    }
}
