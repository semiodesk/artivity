using System;
using Eto.Forms;
using Eto.Drawing;

namespace ArtivityExplorer
{
    public class SetupDialog : Dialog
    {
        #region Members

        public bool Success { get; set; }

        #endregion

        #region Constructors

        public SetupDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        protected void InitializeComponent()
        {
            Title = "Artivity Setup";
            Width = 500;
            Height = 500;
            Padding = new Padding(0);

            Content = new SetupWelcomePage(this);
        }

        #endregion
    }
}

