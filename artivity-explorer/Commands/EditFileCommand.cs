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
using System.Diagnostics;
using System.IO;
using Semiodesk.Trinity;
using Eto.Forms;
using Eto.Drawing;
using Artivity.DataModel;

namespace Artivity.Explorer
{
    public class EditFileCommand : Command
    {
        #region Members

        private string _filePath;

        private SoftwareAgent _fileEditor;

        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                _fileEditor = TryGetEditor(_filePath);

                OnPropertyChanged();
            }
        }

        #endregion

        #region Constructors

        public EditFileCommand()
        {
            MenuText = "Edit";
            ToolBarText = "Edit";
            ToolTip = "Edit the file in the default editor.";
            Image = Bitmap.FromResource("Edit.png");
            Shortcut = Application.Instance.CommonModifier | Keys.Enter;
        }

        #endregion

        #region Methods

        private void OnPropertyChanged()
        {
            Enabled = File.Exists(_filePath) && _fileEditor != null && !string.IsNullOrEmpty(_fileEditor.ExecutableName);
        }

        protected override void OnExecuted(EventArgs e)
        {
            ProcessStartInfo process = new ProcessStartInfo();
            process.Arguments = _filePath;
            process.UseShellExecute = true;
            process.WorkingDirectory = Path.GetDirectoryName(_filePath);
            process.FileName = _fileEditor.ExecutableName;

            Process.Start(process);
        }

        private SoftwareAgent TryGetEditor(string filePath)
        {
            string queryString = @"
                PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                PREFIX nfo: <http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#>
                PREFIX prov: <http://www.w3.org/ns/prov#>

                SELECT ?s ?p ?o WHERE
                {
                       ?activity prov:used ?entity .
                       ?activity prov:qualifiedAssociation ?association .

                       ?association prov:agent ?s .

                       ?s rdf:type prov:SoftwareAgent .
                       ?s ?p ?o .

                       ?entity nfo:fileUrl ""file://" + filePath + @""" .
                }";

            SparqlQuery query = new SparqlQuery(queryString);

            return Models.GetAllActivities().GetResources<SoftwareAgent>(query).FirstOrDefault();
        }

        #endregion
    }
}

