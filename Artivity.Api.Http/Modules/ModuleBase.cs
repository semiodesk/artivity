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

using Semiodesk.Trinity;
using System;
using Nancy;

namespace Artivity.Api.Http
{
	public class ModuleBase : NancyModule
	{
		#region Constructors

		public ModuleBase() {}

		#endregion

		#region Methods

		protected IModel GetModel(Uri uri)
		{
			IStore store = StoreFactory.CreateStoreFromConfiguration("virt0");

			if (store.ContainsModel(uri))
			{
				return store.GetModel(uri);
			}
			else
			{
				return store.CreateModel(uri);
			}
		}

		protected HttpStatusCode LogInfo(HttpStatusCode status, string msg, params object[] p)
		{
			Console.WriteLine("[{0}] Info: ", status, msg, p);

			return status;
		}

		protected HttpStatusCode LogRequest(HttpStatusCode status, string route, string method, string content)
		{
			Console.WriteLine("[{0}] {2} {1}", status, route, method);

			if (!string.IsNullOrEmpty(content))
			{
				Console.WriteLine(content);
			}

			return status;
		}

		protected HttpStatusCode LogError(HttpStatusCode status, Exception e)
		{
			Console.WriteLine("[{0}] Error: {1}: {2}", status, e.GetType(), e.Message);

			return status;
		}

		protected HttpStatusCode LogError(HttpStatusCode status, string msg, params object[] p)
		{
			Console.WriteLine("[{0}] Error: ", status, msg, p);

			return status;
		}

		#endregion
	}
}
