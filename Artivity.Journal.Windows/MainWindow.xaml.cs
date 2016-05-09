using CefSharp;
using CefSharp.Wpf;
using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Artivity.Journal.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            AppearanceManager.Current.AccentColor = Color.FromRgb(249, 76, 0);

            Browser.Loaded += OnBrowserLoaded;
        }

        void OnBrowserLoaded(object sender, RoutedEventArgs e)
        {
            Browser.WebBrowser.LoadError += WebBrowser_LoadError;
        }

        void WebBrowser_LoadError(object sender, CefSharp.LoadErrorEventArgs e)
        {
            try
            {
                string url = "http://artivity.io/errors/no-apid-connection.html";

                DefaultResourceHandlerFactory handler = Browser.ResourceHandlerFactory as DefaultResourceHandlerFactory;

                if (handler == null)
                {
                    throw new Exception("LoadHtml can only be used with the default IResourceHandlerFactory(DefaultResourceHandlerFactory) implementation");
                }
                else if (!handler.Handlers.ContainsKey(url))
                {
                    // NOTE: This somehow only works the first time. Any subsequent requests to the
                    // registered URL fail for an unknown reason.
                    string assemblyName = "Artivity.Apid.Modules.Journal";

                    var assembly = Assembly.Load(assemblyName);
                    var resourceName = assemblyName + ".app.partials.error-no-apid-connection.html";

                    Stream stream = assembly.GetManifestResourceStream(resourceName);

                    handler.RegisterHandler(url, ResourceHandler.FromStream(stream));

                    Browser.Load(url);
                }
            }
            catch (Exception ex)
            {
                Content = new TextBlock() { Text = ex.Message };
            }
        }
    }
}
