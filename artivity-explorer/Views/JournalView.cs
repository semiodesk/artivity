using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Semiodesk.Trinity;
using Eto.Forms;
using Eto.Drawing;
using Artivity.Model;
using Artivity.Model.ObjectModel;
using Artivity.Explorer.Controls;

namespace Artivity.Explorer
{
    public class JournalView : View
    {
        #region Members

        private readonly JournalViewHeader _header = new JournalViewHeader();

        private readonly GridView _grid = new GridView();

        #endregion

        #region Constructors

        public JournalView()
        {
            Orientation = Orientation.Vertical;

            Items.Add(new StackLayoutItem(_header, HorizontalAlignment.Stretch, false));
            Items.Add(new StackLayoutItem(_grid, HorizontalAlignment.Stretch, true));

            GridColumn fileColumn = new GridColumn();
            fileColumn.HeaderText = "File Name";
            fileColumn.DataCell = new TextBoxCell("FileName");
            fileColumn.Width = 395;
            fileColumn.AutoSize = false;

            _grid.Columns.Add(fileColumn);
            _grid.Columns.Add(new GridColumn() { DataCell = new TextBoxCell("FormattedTotalEditingTime"), HeaderText = "Editing Time", Width = 90 });
            _grid.Columns.Add(new GridColumn() { DataCell = new TextBoxCell("FormattedLastEditingDate"), HeaderText = "Last Used", Width = 90 });
            _grid.CellDoubleClick += OnCellDoubleClick;

            Refresh();
        }

        #endregion

        #region Methods

        public void Refresh()
        {
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
            Dictionary<string, JournalViewListItem> items = new Dictionary<string, JournalViewListItem>();

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

                JournalViewListItem item = new JournalViewListItem()
                {
                    Agent = agent,
                    Url = url,
                    Path = path,
                    LastEditingDate = startTime,
                    TotalEditingTime = editingTime
                };
                
                if (items.ContainsKey(path))
                {
                    items[path].TotalEditingTime += item.TotalEditingTime;
                }
                else
                {
                    items[path] = item;
                }
            }

            _grid.DataStore = items.Values;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.Key == Keys.Enter)
            {
                JournalViewListItem selectedItem = _grid.SelectedItem as JournalViewListItem;

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
                JournalViewListItem selectedItem = _grid.SelectedItem as JournalViewListItem;

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
            JournalViewListItem selectedItem = _grid.SelectedItem as JournalViewListItem;

            if (selectedItem == null)
            {
                return;
            }

            MainWindow.Navigate<FileView>((window, view) =>
            {
                view.FilePath = selectedItem.Path;
            });
        }

//        protected void OnCellDoubleClick(object sender, EventArgs e)
//        {
//            JournalItem selectedItem = SelectedItem as JournalItem;
//
//            if (selectedItem == null || !File.Exists(selectedItem.Path))
//            {
//                return;
//            }
//
//            SoftwareAgent agent = Models.GetAgents().GetResource<SoftwareAgent>(selectedItem.Agent);
//
//            if (string.IsNullOrEmpty(agent.ExecutableName))
//            {
//                return;
//            }
//
//            ProcessStartInfo process = new ProcessStartInfo();
//            process.Arguments = selectedItem.Path;
//            process.UseShellExecute = true;
//            process.WorkingDirectory = Path.GetDirectoryName(selectedItem.Path);
//            process.FileName = agent.ExecutableName;
//
//            Process.Start(process);
//        }

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

