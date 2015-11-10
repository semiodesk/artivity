using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eto.Drawing;

namespace Artivity.Explorer.Parsers
{
    public class SvgStats
    {
        #region Members

        public int LayerCount { get; set; }

        public int GroupCount { get; set; }

        public int ElementCount { get; set; }

        public int MaskCount { get; set; }

        public int ClipCount { get; set; }

		private readonly HashSet<Color> _colourKeys = new HashSet<Color>();

        private List<Color> _colours = new List<Color>();

        public IEnumerable<Color> Colours
        {
            get { return _colours; }
        }

        #endregion

        #region Methods

        public void AddColour(Color colour)
        {
			if(colour.A == 0 ||  _colourKeys.Contains(colour)) return;

            _colourKeys.Add(colour);
            _colours.Add(colour);
        }

        internal void SortColours()
        {
            //_colours = _colours.OrderBy(c => c.GetBrightness()).ToList();
        }

        #endregion
    }
}
