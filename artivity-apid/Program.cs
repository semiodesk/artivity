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
using System.IO;
using System.Threading;
using Semiodesk.Trinity;
using Artivity.Model;
using CommandLine;
using Nancy.Hosting.Self;

namespace Artivity.Api.Http
{
	class HttpService
	{
		public const int Port = 8272;

		public static void Main(string[] args)
		{
            SemiodeskDiscovery.Discover();

            Options options = new Options();

            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                return;
            }

            Console.WriteLine("Artivity Logging Service, Version 1.5.9");
            Console.WriteLine();

            AutoResetEvent wait = new AutoResetEvent(false);
            AutoResetEvent finalize = new AutoResetEvent(false);

            Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs e) =>
            {
                Logger.LogInfo("Received SIGINT. Shutting down.");

                wait.Set();
                finalize.WaitOne();
            };

            Thread t = new Thread(() =>
            {
                InitializeModels(options);

                using (var host = new NancyHost(new Uri("http://localhost:" + Port)))
                {
                    try
                    {
                        host.Start();

                        Logger.LogInfo("Started listening on port {0}..", Port);

                        using (var monitor = new FileSystemMonitor())
                        {
                            monitor.Start();

                            wait.WaitOne();
                        }
                    }
                    finally
                    {
                        Logger.LogInfo("Stopped listening on port {0}..", Port);
                    }
                }

                finalize.Set();
            });
            
            t.Start();
            t.Join();
		}

        private static void InitializeModels(Options options)
        {
            IStore store = StoreFactory.CreateStoreFromConfiguration(Models.DefaultStore);

            if (options.Update)
            {
                Logger.LogInfo("Updating ontologies.");

                store.LoadOntologySettings();
            }

            if (!store.ContainsModel(Models.Agents))
            {
                Logger.LogInfo("Creating model {0}..", Models.Agents);

                store.CreateModel(Models.Agents);
            }

            if (!store.ContainsModel(Models.Activities))
            {
                Logger.LogInfo("Creating model {0}..", Models.Activities);

                store.CreateModel(Models.Activities);
            }

            if (!store.ContainsModel(Models.WebActivities))
            {
                Logger.LogInfo("Creating model {0}..", Models.WebActivities);

                store.CreateModel(Models.WebActivities);
            }
        }
	}
}
