using System;
using Artivity.Model;
using ArtivityExplorer.Controls;
using Eto.Forms;
using Eto.Drawing;

namespace ArtivityExplorer
{
	public class Program
	{
        [STAThread]
		static void Main ()
		{
			SemiodeskDiscovery.Discover();

            if (!Setup.HasModels() && !Setup.InstallModels())
            {
                throw new Exception("Failed to setup the database.");
            }

            Application app = new Application();

            using (MainWindow window = new MainWindow())
            {
                app.Run(window);
            }
		}
	}
}

