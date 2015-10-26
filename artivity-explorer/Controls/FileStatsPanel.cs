using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xwt;
using Xwt.Drawing;

namespace ArtivityExplorer.Controls
{
    public class FileStatsPanel : VBox
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

            MinWidth = 250;
            Margin = 0;
            Spacing = 30;

			BackgroundColor = Color.FromBytes(246, 246, 245);
        }

        #endregion

        #region Methods

        private void InitializeComponent()
        {
			EditingWidget.Margin = new WidgetSpacing(10, 10, 20, 0);
			CompositionWidget.Margin = new WidgetSpacing(10, 0, 20, 0);
			ColourWidget.Margin = new WidgetSpacing(10, 0, 20, 0);
            ColourWidget.HorizontalPlacement = WidgetPlacement.Fill;

            PackStart(EditingWidget);
            PackStart(CompositionWidget);
            PackStart(ColourWidget);
        }

        #endregion
    }
}
