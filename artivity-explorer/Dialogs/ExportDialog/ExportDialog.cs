using System;
using System.IO;
using Semiodesk.Trinity;
using Artivity.Model;
using Eto.Forms;

namespace ArtivityExplorer.Dialogs.ExportDialog
{
    public class ExportDialog : Dialog
    {
        private string _filename;

        public ExportDialog(string filename)
        {
            _filename = filename;

            Title = "Exporting..";

            Button exportButton = new Button() { Text = "Cancel" };
            exportButton.Click += OnCancelButtonClicked;

            StackLayout buttons = new StackLayout();
            buttons.Items.Add(exportButton);

            StackLayout layout = new StackLayout();
            layout.Items.Add(new ProgressBar() { Indeterminate = true });
            layout.Items.Add(buttons);

            Content = layout;

            Shown += OnShown;
        }
            
        private void OnShown(object sender, EventArgs e)
        {
            using (FileStream stream = new FileStream(_filename, FileMode.Create))
            {
                IStore store = StoreFactory.CreateStoreFromConfiguration("virt0");

                IModel model = store.GetModel(Models.Activities);
                model.Write(stream, RdfSerializationFormat.RdfXml);

                stream.Close();
            }

            Close();
        }

        private void OnCancelButtonClicked(object sender, EventArgs e)
        {
            Close();
        }
    }
}

