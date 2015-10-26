using System;
using Xwt;
using ArtivityExplorer.Controls;

namespace ArtivityExplorer
{
    public class ExportButton : Button
    {
        public ExportButton()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Label = "Export";
        }

        protected override void OnClicked(EventArgs e)
        {
            base.OnClicked(e);

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filters.Add(new FileDialogFilter("RDF/XML", "*.rdf"));

            if (dialog.Run())
            {
                string file = dialog.FileName;

                ExportDialog export = new ExportDialog(file);
                export.Run();
            }
        }
    }
}

