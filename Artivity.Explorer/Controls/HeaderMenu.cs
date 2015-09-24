using System;
using Xwt;
using Xwt.Drawing;
using ArtivityExplorer.Controls;

namespace ArtivityExplorer
{
    public class HeaderMenu : HBox
    {
        public HeaderMenu(MainWindow window)
        {
            PackStart(new FileButton(window) { MarginTop = 5 });
            PackEnd(new UserButton() { MarginBottom = 5, MarginRight = 14 });
        }
    }
}

