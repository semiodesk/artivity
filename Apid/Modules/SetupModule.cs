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
// Copyright (c) Semiodesk GmbH 2017

using Artivity.Api;
using Artivity.Api.Modules;
using Artivity.Api.Parameters;
using Artivity.Api.Platform;
using Artivity.DataModel;
using Semiodesk.Trinity;
using Semiodesk.Trinity.Store;
using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using Nancy;
using Nancy.IO;
using Nancy.ModelBinding;
using Nancy.Responses;
using Nancy.Extensions;
using Newtonsoft.Json;

namespace Artivity.Apid.Modules
{
    public class SetupModule : ModuleBase
    {
        #region Constructors

        public SetupModule(IModelProvider modelProvider, IPlatformProvider platformProvider)
            : base("/artivity/api/1.0/setup", modelProvider, platformProvider)
        {
            Get["/"] = parameters =>
            {
                return Response.AsJsonSync(platformProvider.DidSetupRun);
            };

            Post["/"] = parameters =>
            {
                string data = Request.Body.AsString();

                Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);

                if (values.ContainsKey("runSetup"))
                {
                    bool value = Convert.ToBoolean(values["runSetup"]);

                    platformProvider.DidSetupRun = value;
                    platformProvider.WriteConfig(platformProvider.Config);

                    return HttpStatusCode.OK;
                }
                else
                {
                    return platformProvider.Logger.LogRequest(HttpStatusCode.BadRequest, Request);
                }
            };
        }

        #endregion
    }
}

