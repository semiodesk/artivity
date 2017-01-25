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
//  Moritz Eberl <moritz@semiodesk.com>
//  Sebastian Faubel <sebastian@semiodesk.com>
//
// Copyright (c) Semiodesk GmbH 2015

using Artivity.Api.Platform;
using Artivity.Apid.Plugin.OSX;
using Artivity.Apid.Platforms;
using Artivity.DataModel;
using System.IO;
using System;

namespace Artivity.Apid.Plugins
{
    public class PluginCheckerFactory
    {
        #region Members

        public static Type _checkerType;

        #endregion

        #region Methods

        public static void RegisterType<T>() where T : PluginChecker
        {
            if (_checkerType == null)
            {
                _checkerType = typeof(T);
            }
            else
            {
                string msg = string.Format("Trying to overwrite registered type: {0}", _checkerType);
                throw new Exception(msg);
            }
        }

        public static PluginChecker CreatePluginChecker(IPlatformProvider platformProvider, IModelProvider modelProvider, DirectoryInfo folder)
        {
            if (_checkerType != null)
            {
                return Activator.CreateInstance(_checkerType, platformProvider, modelProvider, folder) as PluginChecker;
            }
            else
            {
                string msg = string.Format("No type registered for type: {0}", typeof(PluginChecker));
                throw new Exception(msg);
            }
        }

        #endregion
    }
}
