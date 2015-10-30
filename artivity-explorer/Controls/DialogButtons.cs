using System;
using Eto.Forms;

namespace ArtivityExplorer
{
    public class DialogButtons : StackLayout
    {
        #region Members

        public Button OkButton { get; private set; }

        public Button CancelButton { get; private set; }

        #endregion

        #region Constructors

        public DialogButtons()
        {
            Orientation = Orientation.Horizontal;

            Spacing = 7;

            OkButton = new Button();
            OkButton.Width = 100;
            OkButton.Text = "OK";

            CancelButton = new Button();
            CancelButton.Width = 100;
            CancelButton.Text = "Cancel";

            Items.Add(new StackLayoutItem(null, true));
            Items.Add(new StackLayoutItem(CancelButton));
            Items.Add(new StackLayoutItem(OkButton));
        }

        #endregion
    }
}

