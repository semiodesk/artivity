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

using System;
using System.Collections.Generic;
using System.Linq;
using Semiodesk.Trinity;
using Semiodesk.Trinity.Serialization;
using Nancy;
using Artivity.DataModel;
using Nancy.IO;
using System.IO;
using Newtonsoft.Json;
using Artivity.Apid.Platforms;

namespace Artivity.Apid
{
	public class ModuleBase : NancyModule
    {
        #region Members

        public IModelProvider ModelProvider { get; set; }

        public IPlatformProvider PlatformProvider { get; set; }
        
        protected string ArtivityModulePath { get; set; }

        #endregion

        #region Constructors

        public ModuleBase(IModelProvider model, IPlatformProvider platform) 
        {
            ModelProvider = model;
            PlatformProvider = platform;

            After += ctx =>
            {
                model.ReleaseStore();
            };
        }

        public ModuleBase(string modulePath, IModelProvider model, IPlatformProvider platform) : base(modulePath)
        {
            ModelProvider = model;
            PlatformProvider = platform;
            ArtivityModulePath = modulePath;

            After += ctx =>
            {
                model.ReleaseStore();
            };
        }

		#endregion

        #region Methods

        public T Bind<T>(IStore store, RequestStream stream) where T : Resource
        {
            using (var reader = new StreamReader(stream))
            {
                string value = reader.ReadToEnd();

                JsonResourceSerializerSettings settings = new JsonResourceSerializerSettings(store);

                return JsonConvert.DeserializeObject<T>(value, settings);
            }
        }

        protected bool IsUri(string uri, UriKind kind = UriKind.Absolute)
        {
            return Uri.IsWellFormedUriString(uri, kind);
        }

        protected bool IsFileUrl(string url)
        {
            return IsUri(url) | IsUri(Uri.EscapeUriString(url));
        }

        #endregion
	}
}
