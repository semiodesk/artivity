using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eto.Forms;
using Eto.Drawing;

namespace Artivity.Explorer.Controls
{
    public class FileStatsPanel : TableLayout
    {
        #region Members

        public readonly EditingWidget EditingWidget = new EditingWidget();

        public readonly ColourWidget ColourWidget = new ColourWidget();

        public readonly CompositionWidget CompositionWidget = new CompositionWidget();

        #endregion

        #region Constructors

        public FileStatsPanel()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void InitializeComponent()
        {
            Spacing = new Size(30, 30);
            Padding = new Padding(10);

            Rows.Add(new TableRow(new TableCell(EditingWidget, true), new TableCell(CompositionWidget, true)));
            Rows.Add(new TableRow(new TableCell(ColourWidget, true), new TableCell()));
        }

        #endregion
    }
}
