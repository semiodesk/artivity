using System;
using Xwt;
using Semiodesk.Trinity;
using System.IO;
using Artivity.Model;

namespace ArtivityExplorer
{
    public class ExportDialog : Dialog
    {
        private string _filename;

        public ExportDialog(string filename)
        {
            _filename = filename;

            Title = "Exporting..";

            Button exportButton = new Button("Cancel");
            exportButton.Clicked += OnCancelButtonClicked;

            HBox buttons = new HBox();
            buttons.PackEnd(exportButton);

            VBox layout = new VBox();
            layout.PackStart(new ProgressBar() { Indeterminate = true });
            layout.PackStart(buttons);

            Content = layout;
        }

        protected override void OnShown()
        {
            base.OnShown();

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

