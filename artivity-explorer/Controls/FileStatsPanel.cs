using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eto.Forms;
using Eto.Drawing;

namespace Artivity.Explorer.Controls
{
    public class FileStatsPanel : StackLayout
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
            Orientation = Orientation.Vertical;
            Width = 200;
            Spacing = 30;
            Padding = new Padding(10);

            Items.Add(new StackLayoutItem(EditingWidget, HorizontalAlignment.Stretch, false));
            Items.Add(new StackLayoutItem(CompositionWidget, HorizontalAlignment.Stretch, false));
            Items.Add(new StackLayoutItem(ColourWidget, HorizontalAlignment.Stretch, false));
        }

        #endregion
    }
}
