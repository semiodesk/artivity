using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Semiodesk.Trinity;
using Eto.Forms;
using Eto.Drawing;
using Artivity.Model;
using System.Diagnostics;
using Artivity.Model.ObjectModel;

namespace ArtivityExplorer
{
    public class JournalView : GridView
    {
        #region Members

        private readonly Dictionary<string, JournalItem> _items = new Dictionary<string, JournalItem>();

        #endregion

        #region Constructors

        public JournalView()
        {
            RowHeight = 40;

            Columns.Add(new GridColumn() { DataCell = new TextBoxCell("FileName"), HeaderText = "File Name", Width = 300 });
            Columns.Add(new GridColumn() { DataCell = new TextBoxCell("FormattedTotalEditingTime"), HeaderText = "Editing Time", Width = 90 });
            Columns.Add(new GridColumn() { DataCell = new TextBoxCell("FormattedLastEditingDate"), HeaderText = "Last Used", Width = 90 });

            CellDoubleClick += OnCellDoubleClick;

            Refresh();
        }

        #endregion

        #region Methods

        public void Refresh()
        {
            _items.Clear();

            string queryString = @"
                PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                PREFIX nfo: <http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#>
                PREFIX prov: <http://www.w3.org/ns/prov#>
                PREFIX dces: <http://purl.org/dc/elements/1.1/>

                SELECT ?agent ?startTime ?endTime ?fileUrl WHERE
                {
                       ?activity prov:used ?entity .
                       ?activity prov:startedAtTime ?startTime .
                       ?activity prov:endedAtTime ?endTime .
                       ?activity prov:qualifiedAssociation ?association .

                       ?association prov:agent ?agent .

                       ?entity nfo:fileUrl ?fileUrl .
                }
                ORDER BY DESC(?startTime)";

            IModel model = Models.GetActivities();

            SparqlQuery query = new SparqlQuery(queryString);
            ISparqlQueryResult result = model.ExecuteQuery(query);

            LoadBindings(result.GetBindings());
        }

        private void LoadBindings(IEnumerable<BindingSet> bindings)
        {
            foreach (BindingSet binding in bindings)
            {
                string url = binding["fileUrl"].ToString();

                // Skip any malformed URIs.
                if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                {
                    continue;
                }

                // Do not list files which do not exist.
                string path = new Uri(url).AbsolutePath;

                if (!File.Exists(path))
                {
                    continue;
                }

                Uri agent = new Uri(binding["agent"].ToString());
                DateTime startTime = (DateTime)binding["startTime"];
                DateTime endTime = (DateTime)binding["endTime"];
                TimeSpan editingTime = endTime - startTime;

                JournalItem item = new JournalItem()
                {
                    Agent = agent,
                    Url = url,
                    Path = path,
                    LastEditingDate = startTime,
                    TotalEditingTime = editingTime
                };
                
                if (_items.ContainsKey(path))
                {
                    _items[path].TotalEditingTime += item.TotalEditingTime;
                }
                else
                {
                    _items[path] = item;
                }
            }

            DataStore = _items.Values;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.Key == Keys.Enter)
            {
                JournalItem selectedItem = SelectedItem as JournalItem;

                if(selectedItem == null)
                {
                    return;
                }

                string filePath = selectedItem.Path;

                if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                {
                    return;
                }

                RaiseFileSelected(new FileSelectionEventArgs(filePath));
            }
            else if(e.Key == Keys.Space)
            {
                JournalItem selectedItem = SelectedItem as JournalItem;

                if (selectedItem == null)
                {
                    return;
                }

                RaiseFileSelected(new FileSelectionEventArgs(selectedItem.Path));
            }
            else if (e.Key == Keys.F5)
            {
                Refresh();
            }
        }

        protected void OnCellDoubleClick(object sender, EventArgs e)
        {
            JournalItem selectedItem = SelectedItem as JournalItem;

            if (selectedItem == null || !File.Exists(selectedItem.Path))
            {
                return;
            }

            SoftwareAgent agent = Models.GetAgents().GetResource<SoftwareAgent>(selectedItem.Agent);

            if (string.IsNullOrEmpty(agent.ExecutableName))
            {
                return;
            }

            ProcessStartInfo process = new ProcessStartInfo();
            process.Arguments = selectedItem.Path;
            process.UseShellExecute = true;
            process.WorkingDirectory = Path.GetDirectoryName(selectedItem.Path);
            process.FileName = agent.ExecutableName;

            Process.Start(process);
        }

        #endregion

        #region Events

        public FileSelectionEventHandler FileSelected { get; set; }

        private void RaiseFileSelected(FileSelectionEventArgs e)
        {
            if (FileSelected == null) return;

            FileSelected(this, e);
        }

        #endregion
    }
}

