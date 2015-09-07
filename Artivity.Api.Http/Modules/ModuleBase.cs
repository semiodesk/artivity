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

		protected IModel GetModel(string uri)
		{
			IStore store = StoreFactory.CreateStoreFromConfiguration("virt0");

			Uri model = new Uri(uri);

			if (store.ContainsModel(model))
			{
				return store.GetModel(model);
			}
			else
			{
				return store.CreateModel(model);
			}
		}

		protected void LogRequest(string route, string method, string content, HttpStatusCode result)
		{
			Console.WriteLine("");
			Console.WriteLine(">>> Request: {0}; Method: {1}; Result: {2}", route, method, result);
			Console.WriteLine(content);
		}

		protected void LogError(Exception e)
		{
			Console.WriteLine("");
			Console.WriteLine("ERROR: {0}: {1}", e.GetType(), e.Message);
		}

		#endregion
	}
}
