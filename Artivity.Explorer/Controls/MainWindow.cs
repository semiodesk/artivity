using Semiodesk.Trinity;
using System;
using System.Linq;
using System.Collections.Generic;
using Xwt;
using Artivity.Model;
using Artivity.Model.ObjectModel;
using ArtivityExplorer.Parsers;
using System.IO;

namespace ArtivityExplorer.Controls
{
    public class MainWindow : Window
    {
        #region Members

        private HeaderMenu _menu;

        private StatusBar _statusBar = new StatusBar();

        #endregion

        #region Constructors

        public MainWindow() : base()
        {
			InitializeModel();
            InitializeComponent();

            Closed += HandleClosed;
        }

        #endregion

        #region Methods

        private void InitializeModel()
        {
            if (!Setup.HasModels() && !Setup.InstallModels())
            {
                throw new Exception("Failed to setup RDF datamodels.");
            }
        }

        private void InitializeComponent()
        {
            Padding = 0;

            VBox layout = new VBox();
            layout.PackStart(new UserButton() { HorizontalPlacement = WidgetPlacement.End, Margin = 3 });
            layout.PackStart(new TabControl() { MarginTop = -10 }, true);

            Content = layout;
        }

        private void HandleClosed(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion
    }
}
