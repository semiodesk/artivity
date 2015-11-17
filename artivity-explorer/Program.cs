using System;
using Eto.Forms;
using Eto.Drawing;
using Artivity.DataModel;
using Artivity.Explorer.Controls;
using Artivity.Explorer.Dialogs.SetupWizard;

namespace Artivity.Explorer
{
	public class Program
	{
        [STAThread]
        static void Main(string[] args)
		{
			SemiodeskDiscovery.Discover();

            Options options = new Options();

            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                return;
            }

            if (options.SingleUser)
            {
                Models.Agents = new Uri("http://localhost:8890/artivity/1.0/agents");
                Models.Activities = new Uri("http://localhost:8890/artivity/1.0/activities");
                Models.WebActivities = new Uri("http://localhost:8890/artivity/1.0/activities/web");
                Models.Monitoring = new Uri("http://localhost:8890/artivity/1.0/monitoring");
            }

            if (!Setup.HasModels() && !Setup.InstallModels())
            {
                throw new Exception("Failed to setup the database.");
            }

            try
            {
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
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
		}
	}
}

