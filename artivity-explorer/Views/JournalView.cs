using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Semiodesk.Trinity;
using Eto.Forms;
using Eto.Drawing;
using Artivity.DataModel;
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
            Spacing = 14;

            Items.Add(new StackLayoutItem(_header, HorizontalAlignment.Stretch, false));
            Items.Add(new StackLayoutItem(_grid, HorizontalAlignment.Stretch, true));

            _grid.RowHeight = 50;
            _grid.AllowMultipleSelection = false;
            _grid.AllowColumnReordering = false;
            _grid.Columns.Add(new GridColumn() { DataCell = new CanvasThumbnailCell("FilePath"), Width = 75, AutoSize = false, Resizable = false });
            _grid.Columns.Add(new GridColumn() { DataCell = new TextBoxCell("FileName"), HeaderText = "File", Width = 295, AutoSize = false });
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

            IModel model = Models.Instance.Provider.GetActivities();

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
                string path = new Uri(url).LocalPath;

                if (!File.Exists(path))
                {
                    continue;
                }

                UriRef agent = new UriRef(binding["agent"].ToString());
                DateTime startTime = (DateTime)binding["startTime"];
                DateTime endTime = (DateTime)binding["endTime"];
                TimeSpan editingTime = endTime - startTime;

                JournalViewListItem item = new JournalViewListItem()
                {
                    Agent = agent,
                    FileUrl = url,
                    FilePath = path,
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

                string filePath = selectedItem.FilePath;

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

                RaiseFileSelected(new FileSelectionEventArgs(selectedItem.FilePath));
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
                view.FilePath = selectedItem.FilePath;
            });
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

