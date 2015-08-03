using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xwt;
using Xwt.Drawing;

namespace ArtivityExplorer.Controls
{
    public class StatsPanel : VBox
    {
        #region Members

        public readonly EditingWidget EditingWidget = new EditingWidget();

        public readonly ColourWidget ColourWidget = new ColourWidget();

        public readonly CompositionWidget CompositionWidget = new CompositionWidget();

        #endregion

        #region Constructors

        public StatsPanel()
        {
            InitializeComponent();

            MinWidth = 200;
            Margin = 10;
            Spacing = 30;
        }

        #endregion

        #region Methods

        private void InitializeComponent()
        {
            PackStart(EditingWidget);
            PackStart(CompositionWidget);
            PackStart(ColourWidget);
        }

        #endregion
    }
}
