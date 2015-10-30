using System;
using Eto.Forms;

namespace ArtivityExplorer
{
    public class DialogNavigationButtons : StackLayout
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
            Orientation = Orientation.Horizontal;

            Spacing = 7;

            OkButton = new Button();
            OkButton.Width = 100;
            OkButton.Text = "OK";
            OkButton.Visible = false;

            CancelButton = new Button();
            CancelButton.Width = 100;
            CancelButton.Text = "Cancel";

            NextButton = new Button();
            NextButton.Width = 100;
            NextButton.Text = "Next >";

            BackButton = new Button();
            BackButton.Width = 100;
            BackButton.Text = "< Back";

            Items.Add(new StackLayoutItem(null, true));
            Items.Add(new StackLayoutItem(CancelButton));
            Items.Add(new StackLayoutItem(OkButton));
            Items.Add(new StackLayoutItem(NextButton));
            Items.Add(new StackLayoutItem(BackButton));
        }

        #endregion

        #region Methods

        protected override void OnGotFocus(EventArgs e)
        {
            if (OkButton.Visible && OkButton.Enabled)
            {
                OkButton.Focus();
            }
            else if (NextButton.Visible && NextButton.Enabled)
            {
                NextButton.Focus();
            }
            else if (BackButton.Visible && BackButton.Enabled)
            {
                BackButton.Focus();
            }
            else
            {
                CancelButton.Focus();
            }
        }

        #endregion
    }
}

