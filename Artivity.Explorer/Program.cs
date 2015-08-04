using System;
using Xwt;
using Xwt.Drawing;
using ArtivityExplorer.Controls;

namespace ArtivityExplorer
{
	public class Program
	{
		static void Main ()
		{
			Application.Initialize(ToolkitType.Gtk);

            using (MainWindow window = new MainWindow())
			{
				window.Title = "Artivity Explorer";
				window.Icon = Image.FromResource("icon");
				window.Width = 900;
				window.Height = 600;
				window.Show();

				Application.Run();
			};
		}
	}
}
