using System;
using Xwt;
using Xwt.Drawing;
using ArtivityExplorer.Controls;

namespace ArtivityExplorer
{
    public class HeaderMenu : HBox
    {
        public FileButton FileButton { get; private set; }

        public Button ExportButton { get; private set; }

        public Button PreferncesButton { get; private set; }

        public HeaderMenu(MainWindow window)
        {
            Spacing = 3;
            Margin = 3;

            FileButton = new FileButton();
            ExportButton = new ExportButton() { Sensitive = false };
            PreferncesButton = new UserButton();

            PackStart(FileButton);
            PackStart(ExportButton);
            PackEnd(PreferncesButton);
        }
    }
}

