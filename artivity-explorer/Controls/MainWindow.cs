using Semiodesk.Trinity;
using System;
using System.Linq;
using System.Collections.Generic;
using Xwt;
using Artivity.Model;
using Artivity.Model.ObjectModel;
using ArtivityExplorer.Parsers;
using System.IO;
using Xwt.Drawing;

namespace ArtivityExplorer.Controls
{
    public class MainWindow : Window
    {
        #region Members

        private Button _journalButton;

        private Button _exportButton;

        private Button _userButton;

        private JournalView _journalView;

        private FileView _fileView;

        #endregion

        #region Constructors

        public MainWindow() : base()
        {
            InitializeComponent();

            Closed += HandleClosed;
        }

        #endregion

        #region Methods

        private void InitializeComponent()
        {
            Padding = 0;

            _journalButton = new Button();
            _journalButton.Sensitive = false;
            _journalButton.MinWidth = 40;
            _journalButton.MinHeight = 40;
            _journalButton.Image = BitmapImage.FromResource("journal");
            _journalButton.Clicked += OnJournalButtonClicked;

            _exportButton = new Button();
            _exportButton.Visible = false;
            _exportButton.MinWidth = 40;
            _exportButton.MinHeight = 40;
            _exportButton.Image = BitmapImage.FromResource("export");
            _exportButton.Clicked += OnExportButtonClicked;

            _userButton = new UserButton();

            HBox toolbar = new HBox();
            toolbar.ExpandHorizontal = true;
            toolbar.VerticalPlacement = WidgetPlacement.Center;
            toolbar.Margin = new WidgetSpacing(4, 4, 4, 0);
            toolbar.Spacing = 4;
            toolbar.PackStart(_journalButton);
            toolbar.PackStart(_exportButton);
            toolbar.PackEnd(_userButton);

            _journalView = new JournalView();
            _journalView.Visible = true;
            _journalView.FileSelected += OnJournalFileSelected;
            _journalView.SetFocus();

            _fileView = new FileView();
            _fileView.Visible = false;

            VBox layout = new VBox();
            layout.PackStart(toolbar);
            layout.PackStart(_journalView, true);
            layout.PackStart(_fileView, true);

            Content = layout;
        }

        private void OnJournalButtonClicked(object sender, EventArgs e)
        {
            _journalButton.Sensitive = false;
            _journalView.Visible = true;
            _journalView.Update();
            _fileView.Visible = false;
            _exportButton.Visible = false;
        }

        private void OnJournalFileSelected(object sender, FileSelectionEventArgs e)
        {
            _journalButton.Sensitive = true;
            _journalView.Visible = false;
            _fileView.Visible = true;
            _fileView.FileUrl = e.FileName;
            _fileView.Update();
            _exportButton.Visible = true;
        }

        private void OnExportButtonClicked(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filters.Add(new FileDialogFilter("RDF/XML", "*.rdf"));

            if (dialog.Run())
            {
                string file = dialog.FileName;

                ExportDialog export = new ExportDialog(file);
                export.Run();
            }
        }

        private void HandleClosed(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion
    }
}
