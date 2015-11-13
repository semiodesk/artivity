using Semiodesk.Trinity;
using System;
using System.Linq;
using System.Collections.Generic;
using Artivity.DataModel;
using Artivity.Explorer.Parsers;
using Artivity.Explorer.Dialogs.SetupWizard;
using Artivity.Explorer.Dialogs.ExportDialog;
using System.IO;
using Eto.Forms;
using Eto.Drawing;
using System.Diagnostics;

namespace Artivity.Explorer.Controls
{
    public class MainWindow : Form
    {
        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void InitializeComponent()
        {
            Title = "Artivity Explorer";
            Icon = Icon.FromResource("icon" + Setup.GetIconExtension());

            if(Setup.IsWindowsPlatform())
            {
                ClientSize = new Size(550, 600);
            }
            else
            {
                ClientSize = new Size(650, 700);
            }
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);

            Navigate<JournalView>();
        }
        
        public static void Navigate<T>(NavigationEventHandler<T> onComplete = null) where T : View, new()
        {
            MainWindow window = Application.Instance.MainForm as MainWindow;

            if (window == null)
            {
                Debug.WriteLine("Unable to navigate: application main form is not instance of {0}.", typeof(MainWindow));

                return;
            }

            T view = new T();

            window.Content = view;

            if (onComplete != null)
            {
                onComplete(window, view);
            }
        }

        #endregion
    }

    public delegate void NavigationEventHandler<T>(MainWindow window, T view) where T : View;
}
