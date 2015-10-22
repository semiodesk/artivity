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
			Artivity.Model.SemiodeskDiscovery.Discover();

			Application.Initialize(ToolkitType.Gtk);

            if (!SetupHelper.CheckEnvironment())
            {
                ShowSetup();
            }
            else
            {
                ShowExplorer();
            }
		}

        static void ShowSetup()
        {
            using (SetupDialog setup = new SetupDialog())
            {
                setup.Closed += (object sender, EventArgs e) => 
                {
                    if(setup.Success)
                    {
                        ShowExplorer();
                    }
                };
                
                setup.Show();
                setup.Run();
            }
        }

        static void ShowExplorer()
        {
            using (MainWindow window = new MainWindow())
            {
                window.Title = "Artivity Explorer";
                window.Icon = Image.FromResource("icon");
                window.Width = 1100;
                window.Height = 700;
                window.Show();

                Application.Run();
            }
        }
	}
}

