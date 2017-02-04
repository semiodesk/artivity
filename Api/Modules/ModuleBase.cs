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

using Artivity.Api.IO;
using Artivity.Api.Platform;
using Artivity.DataModel;
using System;
using System.IO;
using Semiodesk.Trinity;
using Nancy;
using Nancy.IO;
using Newtonsoft.Json;

namespace Artivity.Api.Modules
{
	public class ModuleBase : NancyModule
    {
        #region Members

        public IModelProvider ModelProvider { get; set; }

        public IPlatformProvider PlatformProvider { get; set; }

        public IModel UserModel { get; private set; }

        protected string ArtivityModulePath { get; set; }

        #endregion

        #region Constructors

        public ModuleBase(IModelProvider model, IPlatformProvider platform) 
        {
            ModelProvider = model;
            PlatformProvider = platform;

            After.AddItemToEndOfPipeline((ctx) =>
            {
                ModelProvider.ReleaseStore();
            });

            OnError.AddItemToEndOfPipeline((ctx, ex) =>
            {
                ModelProvider.ReleaseStore();

                return null;
            });
        }

        public virtual void LoadCurrentUser()
        {
            UserModel = ModelProvider.GetActivities();
        }

        public ModuleBase(string modulePath, IModelProvider model, IPlatformProvider platform)
            : base(modulePath)
        {
            ModelProvider = model;
            PlatformProvider = platform;
            ArtivityModulePath = modulePath;

            After.AddItemToEndOfPipeline((ctx) =>
            {
                ModelProvider.ReleaseStore();
            });

            OnError.AddItemToEndOfPipeline((ctx, ex) =>
            {
                ModelProvider.ReleaseStore();

                return null;
            });
        }

		#endregion

        #region Methods

        public T Bind<T>(IStore store, RequestStream stream) where T : Resource
        {
            using (var reader = new StreamReader(stream))
            {
                string value = reader.ReadToEnd();

                CustomJsonResourceSerializerSettings settings = new CustomJsonResourceSerializerSettings(store);

                return JsonConvert.DeserializeObject<T>(value, settings);
            }
        }

        protected bool IsUri(string uri, UriKind kind = UriKind.Absolute)
        {
            return Uri.IsWellFormedUriString(uri, kind);
        }

        protected bool IsFileUrl(string url)
        {
            return (!string.IsNullOrEmpty(url)) && IsUri(Uri.EscapeUriString(url));
        }

        protected Response ResponseAsJsonSync(object o)
        {
            string result = JsonConvert.SerializeObject(o);

            // Manually convert the result because the default serializer crashes with an exception when
            // trying to serialize the nested data object HttpAuthenticationProtocol. The exception occurs
            // because the connection is already closed when the serializer tries to load the object from the db.
            MemoryStream stream = new MemoryStream();

            StreamWriter writer = new StreamWriter(stream);
            writer.Write(result);
            writer.Flush();

            stream.Position = 0;

            return Response.FromStream(stream, "application/json");
        }

        #endregion
	}
}
