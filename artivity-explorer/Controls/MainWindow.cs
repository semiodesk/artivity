using Semiodesk.Trinity;
using System;
using System.Linq;
using System.Collections.Generic;
using Artivity.Model;
using Artivity.Model.ObjectModel;
using ArtivityExplorer.Parsers;
using System.IO;
using Eto.Forms;
using Eto.Drawing;

namespace ArtivityExplorer.Controls
{
    public class MainWindow : Form
    {
        #region Members

        private ButtonToolItem _homeButton;

        private ButtonToolItem _exportButton;

        private ButtonToolItem _settingsButton;

        #endregion

        #region Constructors

        public MainWindow()
        {
            InitializeEnvironment();
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void InitializeEnvironment()
        {
            if (Setup.CheckEnvironment())
            {
                return;
            }

            Visible = false;

            using (SetupDialog setup = new SetupDialog())
            {
                setup.ShowModal(this);

                if (setup.Success)
                {
                    Visible = true;
                }
                else
                {
                    Close();
                }
            }
        }

        private void InitializeComponent()
        {
            // Run the setup if not already done.
            Title = "Artivity Explorer";
            Icon = Icon.FromResource("icon");
            ClientSize = new Size(700, 750);

            _homeButton = new HomeToolItem();
            _homeButton.Click += OnHomeButtonClicked;

            _exportButton = new ExportToolItem();
            _exportButton.Click += OnExportButtonClicked;

            _settingsButton = new SettingToolItem();

            ToolBar = new ToolBar();
            ToolBar.TextAlign = ToolBarTextAlign.Right;
            ToolBar.Items.Add(_homeButton);
            ToolBar.Items.AddSeparator(0, SeparatorToolItemType.Divider);
            ToolBar.Items.Add(_exportButton);
            ToolBar.Items.AddSeparator(0, SeparatorToolItemType.FlexibleSpace);
            ToolBar.Items.Add(_settingsButton);

            JournalView journalView = new JournalView();
            journalView.FileSelected += OnJournalFileSelected;

            Content = journalView;
        }

        private void OnHomeButtonClicked(object sender, EventArgs e)
        {
            _homeButton.Enabled = false;

            Content = new JournalView();
        }

        private void OnJournalFileSelected(object sender, FileSelectionEventArgs e)
        {
            _homeButton.Enabled = true;

            FileView view = new FileView();
            view.FileUrl = e.FileName;

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
