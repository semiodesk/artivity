using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eto.Forms;
using Eto.Drawing;

namespace ArtivityExplorer.Controls
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

            Width = 250;
            Spacing = 30;

			BackgroundColor = new Color(246, 246, 245);
        }

        #endregion

        #region Methods

        private void InitializeComponent()
        {
            Items.Add(new StackLayoutItem(EditingWidget));
            Items.Add(new StackLayoutItem(CompositionWidget));
            Items.Add(new StackLayoutItem(ColourWidget));
        }

        #endregion
    }
}
