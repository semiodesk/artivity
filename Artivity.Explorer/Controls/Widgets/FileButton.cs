using System;
using System.Linq;
using System.IO;
using Xwt;
using Xwt.Drawing;
using Semiodesk.Trinity;
using Artivity.Model;
using Artivity.Model.ObjectModel;
using ArtivityExplorer.Controls;

namespace ArtivityExplorer
{
    public class FileButton : Button
    {
        MainWindow _window;

        public FileButton(WindowFrame window)
        {
            InitializeComponent();

            _window = window as MainWindow;
        }

        private void InitializeComponent()
        {
            Style = ButtonStyle.Borderless;

            Label = " Please click here to select a file.";

            Image = BitmapImage.FromResource("picture").WithSize(30, 30);
            ImagePosition = ContentPosition.Left;
        }

        protected override void OnClicked(EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filters.Add(new FileDialogFilter("All files", "*.*"));

            if(dialog.Run())
            {
                Label = " " + dialog.FileName;

                if (_window != null)
                {
                    _window.HandleFileSelected(this, new FileSelectionEventArgs(dialog.FileName));
                }
            }
        }

        #region Events

        public FileSelectionEventHandler FileSelected { get; set; }

        private void RaiseFileSelected(string filename)
        {
            if (FileSelected == null) return;

            FileSelected(this, new FileSelectionEventArgs(filename));
        }

        #endregion
    }
}

