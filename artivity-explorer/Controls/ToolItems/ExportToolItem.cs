using System;
using System.Linq;
using System.IO;
using Eto.Forms;
using Eto.Drawing;
using Semiodesk.Trinity;
using Artivity.Model;
using Artivity.Model.ObjectModel;

namespace Artivity.Explorer
{
    public class ExportToolItem : ButtonToolItem
    {
        #region Constructors

        public ExportToolItem()
        {
            Image = Bitmap.FromResource("export");

            Enabled = false;
        }

        #endregion
    }
}

