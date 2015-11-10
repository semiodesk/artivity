using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eto.Forms;
using Eto.Drawing;

namespace Artivity.Explorer.Controls
{
    public class ColourBox : StackLayout
    {
        #region Constructors

        public ColourBox()
        {
            Spacing = 7;
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
                Panel p = new Panel();
                p.BackgroundColor = c;
                p.MinimumSize = new Size(w, Height);

                Items.Add(new StackLayoutItem(p, HorizontalAlignment.Stretch, true));
            }
        }

        #endregion
    }
}
