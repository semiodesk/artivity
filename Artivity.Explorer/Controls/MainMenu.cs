using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xwt;
using Xwt.Backends;
using Xwt.Drawing;

namespace ArtivityExplorer.Controls
{
    public class MainMenu : Menu
    {
        #region Members

        public static readonly Command OpenCommand = new Command("Open");

        public static readonly Command ExportCommand = new Command("Export");

        public static readonly Command QuitCommand = new Command("Quit");

        #endregion

        #region Constructors

        public MainMenu()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void InitializeComponent()
        {
            MenuItem openItem = new MenuItem(OpenCommand);
            openItem.Clicked += OnOpenClicked;

            MenuItem exportItem = new MenuItem(ExportCommand);
            exportItem.Clicked += OnExportClicked;

            MenuItem quitItem = new MenuItem(QuitCommand);
            quitItem.Clicked += OnQuitClicked;

            MenuItem fileMenu = new MenuItem("File");
            fileMenu.SubMenu = new Menu();
            fileMenu.SubMenu.Items.Add(openItem);
            fileMenu.SubMenu.Items.Add(exportItem);
            fileMenu.SubMenu.Items.Add(new SeparatorMenuItem());
            fileMenu.SubMenu.Items.Add(quitItem);

            Items.Add(fileMenu);

            MenuItem aboutItem = new MenuItem();
            aboutItem.Clicked += OnAboutClicked;

            MenuItem helpMenu = new MenuItem("Help");
            helpMenu.SubMenu = new Menu();
            helpMenu.SubMenu.Items.Add(aboutItem);

            Items.Add(helpMenu);
        }

        private void OnOpenClicked(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filters.Add(new FileDialogFilter("Scalable Vector Graphics (.svg)", "*.svg"));
            
            if(dialog.Run())
            {
                RaiseFileSelected(dialog.FileName);
            }
        }

        private void OnExportClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnQuitClicked(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OnAboutClicked(object sender, EventArgs e)
        {
        }

        #endregion

        #region Events

        public FileSelectionEventHandler FileSelected { get; set; }

        private void RaiseFileSelected(string filename)
        {
            if (FileSelected == null) return;

            FileSelected(this, new FileSelectionEventArgs(filename));
        }

        #endregion
    }

    public class FileSelectionEventArgs : EventArgs
    {
        #region Members

        public readonly string FileName;

        #endregion

        #region Constructors

        public FileSelectionEventArgs(string filename)
        {
            FileName = filename;
        }

        #endregion
    }

    public delegate void FileSelectionEventHandler(object sender, FileSelectionEventArgs e);
}
