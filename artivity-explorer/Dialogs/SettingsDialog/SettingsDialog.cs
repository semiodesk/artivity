using System;
using System.Linq;
using System.IO;
using Eto.Forms;
using Eto.Drawing;
using Semiodesk.Trinity;
using Artivity.DataModel;
using Artivity.Explorer.Controls;

namespace Artivity.Explorer.Dialogs.SettingsDialog
{
    public class SettingsDialog : Wizard
    {
        #region Constructors

        public SettingsDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        protected override void InitializeComponent()
        {
            base.InitializeComponent();

            Size = new Size(450, 450);
            ClientSize = new Size(450, 450);

            LayoutRoot.Padding = new Padding(7);

            CurrentPage = new ContentPage(this);
        }

        #endregion
    }
}

