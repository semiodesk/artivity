using System;
using System.Linq;
using System.IO;
using Eto.Forms;
using Eto.Drawing;
using Semiodesk.Trinity;
using Artivity.Model;
using Artivity.Model.ObjectModel;

namespace ArtivityExplorer
{
    public class HomeToolItem : ButtonToolItem
    {
        #region Constructors

        public HomeToolItem()
        {
            Image = Bitmap.FromResource("journal");

            Enabled = false;
        }

        #endregion
    }
}

