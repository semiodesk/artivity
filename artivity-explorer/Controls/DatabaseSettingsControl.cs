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
//  Sebastian Faubel <sebastian@semiodesk.com>
//
// Copyright (c) Semiodesk GmbH 2015

using System;
using System.Linq;
using System.Collections.Generic;
using Semiodesk.Trinity;
using Eto.Forms;
using Artivity.DataModel;

namespace Artivity.Explorer
{
    public class DatabaseSettingsControl : StackLayout
    {
        #region Constructors

        public DatabaseSettingsControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void InitializeComponent()
        {
            Orientation = Orientation.Vertical;

            Spacing = 14;

            SparqlQuery query = new SparqlQuery("select count(?s) as ?tripleCount where { ?s ?p ?o . }");

            IEnumerable<BindingSet> bindings = Models.GetAll().ExecuteQuery(query).GetBindings();

            int tripleCount = bindings.Any() ? Convert.ToInt32(bindings.First()["tripleCount"]) : -1;
            double estimatedSize = tripleCount * 3 / 1024;

            TableLayout layout = new TableLayout();
            layout.Width = 200;
            layout.Rows.Add(new TableRow(new TableCell(new Label() { Text = "Content" }) { ScaleWidth = true }, new TableCell(new Label() { Text = tripleCount.ToString() + " facts", TextAlignment = TextAlignment.Right })));
            layout.Rows.Add(new TableRow(new TableCell(new Label() { Text = "Estimated size" }) { ScaleWidth = true }, new TableCell(new Label() { Text = estimatedSize.ToString() + " mb", TextAlignment = TextAlignment.Right })));

            Items.Add(new StackLayoutItem(layout));
        }

        #endregion
    }
}