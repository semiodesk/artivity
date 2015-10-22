using System;
using Xwt;

namespace ArtivityExplorer
{
    public class DialogNavigationButtons : HBox
    {
        #region Members

        public Button OkButton { get; private set; }

        public Button CancelButton { get; private set; }

        public Button BackButton { get; private set; }

        public Button NextButton { get; private set; }

        #endregion

        #region Constructors

        public DialogNavigationButtons()
        {
            OkButton = new Button();
            OkButton.MinWidth = 100;
            OkButton.Label = "OK";

            CancelButton = new Button();
            CancelButton.MinWidth = 100;
            CancelButton.Label = "Cancel";

            NextButton = new Button();
            NextButton.MinWidth = 100;
            NextButton.Label = "Next >";

            BackButton = new Button();
            BackButton.MinWidth = 100;
            BackButton.Label = "< Back";

            Margin = 14;
            Spacing = 7;
            PackEnd(CancelButton);
            PackEnd(OkButton);
            PackEnd(NextButton);
            PackEnd(BackButton);

            Show();
        }

        #endregion

        #region Methods

        protected override void OnGotFocus(EventArgs e)
        {
            if (OkButton.Visible && OkButton.Sensitive)
            {
                OkButton.SetFocus();
            }
            else if (NextButton.Visible && NextButton.Sensitive)
            {
                NextButton.SetFocus();
            }
            else if (BackButton.Visible && BackButton.Sensitive)
            {
                BackButton.SetFocus();
            }
            else
            {
                CancelButton.SetFocus();
            }
        }

        #endregion
    }
}

