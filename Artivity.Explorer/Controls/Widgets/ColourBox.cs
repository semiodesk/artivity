using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Xwt;

namespace ArtivityExplorer.Controls
{
    public class ColourBox : HBox
    {
        #region Constructors

        public ColourBox()
        {
            Spacing = 0;
            MinHeight = 20;
        }

        #endregion

        #region Methods

        public void Update(IEnumerable<Color> colours)
        {
            while(Children.Count() > 0)
            {
                Remove(Children.First());
            }

            int n = colours.Count();

			if (n == 0) return;

            int w = Convert.ToInt32(Math.Max(Size.Width / n, 1));

            foreach(Color c in colours)
            {
				PackStart(new Canvas() { BackgroundColor = c.ToXwtColor(), Margin = 0, MinWidth = 1, WidthRequest = w });
            }
        }

        #endregion
    }
}
