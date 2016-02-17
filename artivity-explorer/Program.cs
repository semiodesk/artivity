using System;
using Eto.Forms;
using Eto.Drawing;
using Artivity.DataModel;
using Artivity.Explorer.Controls;
using Artivity.Explorer.Dialogs.SetupWizard;
using System.Configuration;

namespace Artivity.Explorer
{
	public class Program
	{
        [STAThread]
        static void Main(string[] args)
		{
			SemiodeskDiscovery.Discover();

            Models.Instance.ConnectionString = GetConnectionStringFromConfiguration();
            Models.Instance.InitializeStore();

            Options options = new Options();

            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                return;
            }

            if (options.SingleUser)
            {
                Models.Instance.Provider.Agents = new Uri("http://localhost:8890/artivity/1.0/agents");
                Models.Instance.Provider.Activities = new Uri("http://localhost:8890/artivity/1.0/activities");
                Models.Instance.Provider.WebActivities = new Uri("http://localhost:8890/artivity/1.0/activities/web");
                Models.Instance.Provider.Monitoring = new Uri("http://localhost:8890/artivity/1.0/monitoring");
            }

            if (!Setup.HasModels())
            {
                if (!Setup.InstallModels())
                {
                    throw new Exception("Failed to setup the database.");
                }
            }
            else
            {
                Setup.VerfiyIntegrity();
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

        private static string GetConnectionStringFromConfiguration()
        {
            string name = "virt0";
            foreach (ConnectionStringSettings setting in ConfigurationManager.ConnectionStrings)
            {
                if (!string.IsNullOrEmpty(name) && setting.Name != name)
                    continue;

                return setting.ConnectionString;

            }
            return null;
        }
	}
}

