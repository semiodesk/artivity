using System;
using Xwt;

namespace ArtivityExplorer
{
    public class DialogButtons : HBox
    {
        #region Members

        public Button OkButton { get; private set; }

        public Button CancelButton { get; private set; }

        #endregion

        #region Constructors

        public DialogButtons()
        {
            OkButton = new Button();
            OkButton.MinWidth = 100;
            OkButton.Label = "OK";

            CancelButton = new Button();
            CancelButton.MinWidth = 100;
            CancelButton.Label = "Cancel";

            Spacing = 7;
            PackEnd(CancelButton);
            PackEnd(OkButton);

            Show();
        }

        #endregion
    }
}

