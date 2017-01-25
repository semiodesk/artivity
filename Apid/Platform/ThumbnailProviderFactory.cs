﻿// LICENSE:
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
//  Moritz Eberl <moritz@semiodesk.com>
//  Sebastian Faubel <sebastian@semiodesk.com>
//
// Copyright (c) Semiodesk GmbH 2017

using Artivity.Api.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artivity.Apid.Platform
{
    public static class ThumbnailProviderFactory
    {
        #region Members

        public static Type _providerType;

        #endregion

        #region Methods

        public static void RegisterType<T>() where T : IThumbnailProvider
        {
            if (_providerType == null)
            {
                _providerType = typeof(T);
            }
            else
            {
                string msg = string.Format("Trying to overwrite registered type: {0}", _providerType);
                throw new Exception(msg);
            }
        }

        public static IThumbnailProvider CreateProvider()
        {
            if(_providerType != null)
            {
                return Activator.CreateInstance(_providerType) as IThumbnailProvider;
            }
            else
            {
                string msg = string.Format("No type registered for type: {0}", typeof(IThumbnailProvider));
                throw new Exception(msg);
            }
        }

        #endregion
    }
}
