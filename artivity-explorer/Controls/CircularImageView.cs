// LICENSE:
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// AUTHORS:
//
//  Sebastian Faubel <sebastian@semiodesk.com>
//
// Copyright (c) Semiodesk GmbH 2015

using System;
using Eto.Forms;
using Eto.Drawing;

namespace Artivity.Explorer
{
    public class CircularImageView : Drawable
    {
        #region Members

        private Image _buffer;

        private Image _image;

        public Image Image
        {
            get { return _image; }
            set
            {
                _image = value;

                UpdateBuffer();

                Invalidate();
            }
        }

        public float Opacity { get; set; }

        #endregion

        #region Constructors

        public CircularImageView()
        {
            Opacity = 1.0f;
        }

        #endregion

        #region Methods

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_buffer != null)
            {
                TextureBrush brush = new TextureBrush(_buffer, Opacity);

                e.Graphics.FillEllipse(brush, e.ClipRectangle);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            UpdateBuffer();
        }

        private void UpdateBuffer()
        {
            _buffer = _image != null ? new Bitmap(_image, Width, Height, ImageInterpolation.High) : null;
        }

        #endregion
    }
}

