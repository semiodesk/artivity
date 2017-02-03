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
using Artivity.Apid.Modules;
using Artivity.Apid.Plugins;
using Artivity.Apid.Synchronization;
using Artivity.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Diagnostics;
using Nancy.Responses;
using Nancy.Serialization.JsonNet;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Artivity.Apid
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        #region Members

        private TinyIoCContainer _container;

        public IModelProvider ModelProvider { get; set; }

        public IPlatformProvider PlatformProvider { get; set; }

        public PluginChecker PluginChecker { get; set; }

        public IOnlineServiceSynchronizationProvider SynchronizationProvider { get; set; }

        #endregion

        #region Methods

        protected override DiagnosticsConfiguration DiagnosticsConfiguration
        {
            get { return new DiagnosticsConfiguration { Password = @"abc" }; }
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            _container = container;

            if (ModelProvider != null)
            {
                _container.Register(ModelProvider);
            }

            if (PlatformProvider != null)
            {
                _container.Register(PlatformProvider);
            }

            if (PluginChecker != null)
            {
                _container.Register(PluginChecker);
            }

            if(SynchronizationProvider != null)
            {
                _container.Register(SynchronizationProvider);
            }
                
            container.Register<JsonNetSerializer>();
        }
            
        protected override void RequestStartup(Nancy.TinyIoc.TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            #if DEBUG
            pipelines.AfterRequest.AddItemToEndOfPipeline((ctx) =>
            {
                ctx.Response.WithHeader("Access-Control-Allow-Origin", "*")
                            .WithHeader("Access-Control-Allow-Methods", "POST,GET")
                            .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type");

            });
            #endif
        }

        protected override void ConfigureConventions(Nancy.Conventions.NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);

            // Configure Nancy to support serving content from embedded resources.
            foreach(IViewModule view in _container.ResolveAll<IViewModule>())
            {
                Assembly assembly = view.GetType().Assembly;

                string assemblyName = assembly.GetName().Name;
                string[] resourceNames = assembly.GetManifestResourceNames();

                nancyConventions.StaticContentsConventions.Add((context, path) =>
                {
                    if(!context.Request.Path.StartsWith(view.Path))
                    {
                        return null;
                    }

                    EmbeddedFileResponse response = null;

                    string fileName = Path.GetFileName(context.Request.Path);

                    string resourceId = context.Request.Path.TrimEnd('/').Replace(view.Path.TrimEnd('/'), view.Namespace);

                    if (!string.IsNullOrEmpty(resourceId))
                    {
                        resourceId = resourceId.Replace('/', '.');
                        resourceId = resourceId.Replace('\\', '.');
                        resourceId = resourceId.Replace(' ', '_');
                        resourceId = resourceId.Trim('.');

                        #if !OSX
                        int n = fileName.Split('.').Length - 1;

                        string[] segments = resourceId.Split('.');

                        for (int i = 0; i < segments.Length - n - 1; i++)
                        {
                            segments[i] = segments[i].Replace('-', '_');
                        }

                        resourceId = string.Join(".", segments);
                        #endif
                    }

                    if(!string.IsNullOrEmpty(fileName))
                    {
                        fileName = fileName.Replace(' ', '_');
                    }
                    else
                    {
                        fileName = view.DocumentIndex;
                    }

                    if (resourceNames.Contains(resourceId))
                    {
                        string resourcePath = resourceId.Substring(0, resourceId.Length - fileName.Length - 1);

                        response = new EmbeddedFileResponse(assembly, resourcePath, fileName);
                    }
                    else if(resourceNames.Contains(resourceId + '.' + view.DocumentIndex))
                    {
                        response = new EmbeddedFileResponse(assembly, resourceId, fileName);
                    }

                    return response;
                });
            }
        }

        #endregion
    }

}