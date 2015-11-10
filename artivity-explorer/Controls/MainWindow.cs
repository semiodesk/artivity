using Semiodesk.Trinity;
using System;
using System.Linq;
using System.Collections.Generic;
using Artivity.Model;
using Artivity.Model.ObjectModel;
using Artivity.Explorer.Parsers;
using Artivity.Explorer.Dialogs.SetupWizard;
using Artivity.Explorer.Dialogs.ExportDialog;
using System.IO;
using Eto.Forms;
using Eto.Drawing;

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
                ClientSize = new Size(500, 550);
            }
            else
            {
                ClientSize = new Size(600, 650);
            }

            JournalView journalView = new JournalView();
            journalView.FileSelected += OnJournalFileSelected;

            StackLayout layout = new StackLayout();
            layout.Orientation = Orientation.Vertical;
            layout.Items.Add(new StackLayoutItem(new UserHeader(), HorizontalAlignment.Stretch, false));
            layout.Items.Add(new StackLayoutItem(journalView, HorizontalAlignment.Stretch, true));

            Content = layout;
        }

        private void OnHomeButtonClicked(object sender, EventArgs e)
        {
            Content = new JournalView();
        }

        private void OnJournalFileSelected(object sender, FileSelectionEventArgs e)
        {
            FileView view = new FileView();
            view.FileUrl = e.FileName;
            view.Update();

            Content = view;
        }

        private void OnExportButtonClicked(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filters.Add(new FileDialogFilter("RDF/XML", "*.rdf"));

            if (dialog.ShowDialog(this) == DialogResult.Ok)
            {
                string file = dialog.FileName;

                ExportDialog export = new ExportDialog(file);
                export.ShowModalAsync();
            }
        }

        #endregion
    }
}
