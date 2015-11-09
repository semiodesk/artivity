using System;
using Artivity.Model;
using ArtivityExplorer.Controls;
using Eto.Forms;
using Eto.Drawing;
using ArtivityExplorer.Dialogs.SetupWizard;

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
				if (!Setup.CheckEnvironment())
				{
					using (SetupWizard setup = new SetupWizard())
					{
						setup.ShowModal();

						if (setup.Result != DialogResult.Ok)
						{
							return;
						}
					}
				}

				app.Run(window);
			}
		}
	}
}

