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
// Copyright (c) Semiodesk GmbH 2015

using Artivity.DataModel;
using Artivity.DataModel.Journal;
using Nancy;
using Nancy.Responses;
using Nancy.ModelBinding;
using Nancy.IO;
using Nancy.Json;
using Semiodesk.Trinity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using Artivity.Apid.Parameters;
using Artivity.Apid.Accounts;
using Artivity.Apid.Platforms;
using System.Globalization;
using System.Threading;
using Artivity.Api.Plugin;

namespace Artivity.Apid.Modules
{
    public class PluginModule : ModuleBase
    {
        #region Members
        PluginChecker _checker;
        #endregion 

        #region Constructors

        public PluginModule(PluginChecker checker, IModelProvider modelProvider, IPlatformProvider platform)
            : base("/artivity/api/1.0/plugins", modelProvider, platform)
        {
            _checker = checker;
           
            Get["/list"] = paramters =>
            {
                return ListPlugins();
            };

            Get["/install"] = paramters =>
            {
                string uri = Request.Query["uri"];
                return InstallPlugin(uri);
            };
    
        }

        #endregion

        #region Methods

        public Response ListPlugins()
        {
            return Response.AsJson(_checker.Plugins);
        }

        public Response InstallPlugin(string uri)
        {
            _checker.InstallPlugin(uri);
            return null;
        }



        public Response UninstallPlugin()
        {
            return null;
        }

      
        #endregion
    }
}
