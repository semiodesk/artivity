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
using Nancy;
using Artivity.DataModel;

namespace Artivity.Api.Http
{
	public class ModuleBase : NancyModule
	{
		#region Constructors

		public ModuleBase() {}

        public ModuleBase(string modulePath) : base(modulePath) {}

		#endregion

        #region Methods

        protected void UpdateMonitoring()
        {
            if (!IsMonitoringRequired()) return;

            IModel monitoring = Models.GetMonitoring();

            Database database = monitoring.GetResources<Database>().FirstOrDefault();

            DatabaseState state = monitoring.CreateResource<DatabaseState>();
            state.Time = DateTime.Now;
            state.FileSize = database.GetFileSize();
            state.FactsCount = database.GetFactsCount();
            state.Commit();

            database.States.Add(state);
            database.Commit();
        }

        private bool IsMonitoringRequired()
        {
            string queryString = @"
                PREFIX art: <http://semiodesk.com/artivity/1.0/>

                select ?enabled ?time where
                {
                   ?database art:isMonitoringEnabled ?enabled .

                   optional
                   {
                      ?database art:hadState ?state .
                      ?state art:atTime ?time .
                   }
                }
                order by desc(?time) limit 1";

            SparqlQuery query = new SparqlQuery(queryString);

            IEnumerable<BindingSet> bindings = Models.GetMonitoring().ExecuteQuery(query).GetBindings();

            if(bindings.Any())
            {
                BindingSet binding = bindings.First();

                var enabled = Convert.ToBoolean(binding["enabled"]);
                var lastTime = binding["time"].ToString();

                return enabled && (string.IsNullOrEmpty(lastTime) || (DateTime.Now - DateTime.Parse(lastTime)).Minutes >= 1);
            }
            else
            {
                return false;
            }
        }

        #endregion
	}
}
