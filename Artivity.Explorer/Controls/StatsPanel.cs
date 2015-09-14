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
            Margin = 0;
            Spacing = 30;

			BackgroundColor = Color.FromBytes(229, 236, 238);
        }

        #endregion

        #region Methods

        private void InitializeComponent()
        {
			EditingWidget.Margin = new WidgetSpacing(10, 10, 10, 0);
			CompositionWidget.Margin = new WidgetSpacing(10, 0, 10, 0);
			ColourWidget.Margin = new WidgetSpacing(10, 0, 10, 0);

            PackStart(EditingWidget);
            PackStart(CompositionWidget);
            PackStart(ColourWidget);
        }

        #endregion
    }
}
