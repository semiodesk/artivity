using System;
using Xwt;
using Xwt.Drawing;
using ArtivityExplorer.Controls;

namespace ArtivityExplorer
{
    public class HeaderMenu : HBox
    {
        public Button FileButton { get; private set; }

        public Button ExportButton { get; private set; }

        public Button PreferncesButton { get; private set; }

        public HeaderMenu(MainWindow window)
        {
            Spacing = 3;
            Margin = 3;

            FileButton = new FileButton(window);
            ExportButton = new ExportButton(window) { Sensitive = false };
            PreferncesButton = new UserButton();

            PackStart(FileButton);
            PackStart(ExportButton);
            PackEnd(PreferncesButton);
        }
    }
}

