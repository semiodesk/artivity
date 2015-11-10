using System;
using Eto.Forms;
using Eto.Drawing;
using Artivity.Model;
using Artivity.Explorer.Controls;
using Artivity.Explorer.Dialogs.SetupWizard;

namespace Artivity.Explorer
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

