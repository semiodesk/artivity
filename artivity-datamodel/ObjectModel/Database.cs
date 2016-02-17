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
//  Sebastian Faubel <sebastian@semiodesk.com>
//
// Copyright (c) Semiodesk GmbH 2015

using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Semiodesk.Trinity;
using System.Linq;

namespace Artivity.DataModel
{
    [RdfClass(ART.Database)]
    public class Database : Resource
    {
        #region Members

        [RdfProperty(NFO.fileUrl)]
        public string Url { get; set; }

        [RdfProperty(ART.isMonitoringEnabled)]
        public bool IsMonitoringEnabled { get; set; }

        [RdfProperty(ART.hadState)]
        public ObservableCollection<DatabaseState> States { get; private set; }

        #endregion

        #region Constructors

        public Database(Uri uri) : base(uri) {}

        #endregion

        #region Methods

        public long GetFileSize()
        {
            string path = new Uri(Url).AbsolutePath;

            if (File.Exists(path))
            {
                return new FileInfo(path).Length;
            }
            else
            {
                return -1;
            }
        }

        public long GetFactsCount(IModelProvider provider)
        {
            SparqlQuery query = new SparqlQuery("select count(?s) as ?factsCount where { ?s ?p ?o . }");

            IEnumerable<BindingSet> bindings = provider.GetAllActivities().ExecuteQuery(query).GetBindings();

            return bindings.Any() ? Convert.ToInt32(bindings.First()["factsCount"]) : -1;
        }

        #endregion
    }
}
    