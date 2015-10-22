using System;
using Xwt;

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
            Padding = 0;

            DialogPage page = new WelcomePage(this);

            Content = page;
        }

        #endregion
    }
}

