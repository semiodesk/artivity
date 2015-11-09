using System;
using Eto.Forms;
using Eto.Drawing;
using ArtivityExplorer.Controls;

namespace ArtivityExplorer.Dialogs.SetupWizard
{
    public class SetupWizard : Wizard
    {
        #region Constructors

		public SetupWizard()
        {
            Title = "Artivity Setup";

			Size = new Size(550, 450);
			ClientSize = new Size(550, 450);
            BackgroundColor = Colors.White;
            ShowInTaskbar = true;

			InitializeComponent();

            CurrentPage = new WelcomePage(this);
			CurrentPage.Content.BackgroundColor = Colors.White;
        }

        #endregion
    }
}

