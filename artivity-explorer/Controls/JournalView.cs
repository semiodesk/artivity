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
    public class JournalView : Scrollable
    {
        #region Members

        private readonly DynamicLayout _layout = new DynamicLayout() { BackgroundColor = Colors.White, Spacing = new Size(0, 7), DefaultPadding = new Padding(0, 7) };

        private List<JournalFileList> _lists = new List<JournalFileList>();

        private Dictionary<DateTime, JournalFileList> _dayLists = new Dictionary<DateTime, JournalFileList>();

        private Dictionary<string, JournalFileList> _fileLists = new Dictionary<string, JournalFileList>();

        private JournalFileList _selectedList = null;

        #endregion

        #region Constructors

        public JournalView()
        {
            Content = _layout;

            Refresh();
        }

        #endregion

        #region Methods

        public void Clear()
        {
            _layout.Clear();

            foreach (JournalFileList view in _lists)
            {
                view.SelectionChanged -= OnFileListSelectionChanged;
            }

            _lists.Clear();
            _dayLists.Clear();
            _fileLists.Clear();
        }

        public void Refresh()
        {
            if (_layout.Children.Any())
            {
                Clear();
            }

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

                JournalFileListItem item = new JournalFileListItem()
                {
                    Agent = agent,
                    Url = url,
                    Path = path,
                    LastEditingDate = startTime,
                    TotalEditingTime = editingTime
                };
                
                if (_fileLists.ContainsKey(path))
                {                   
                    _fileLists[path].Add(item);
                }
                else
                {
                    DateTime day = startTime.RoundToDay();

                    JournalFileList list;

                    if (!_dayLists.ContainsKey(day))
                    {
                        list = new JournalFileList(this);
                        list.SelectionChanged += OnFileListSelectionChanged;
                        list.CellDoubleClick += OnFileListCellDoubleClicked;

                        _lists.Add(list);
                        _dayLists[day] = list;

                        Label label = new Label() { Text = day.ToString("D").ToUpperInvariant() };
                        label.Font = SystemFonts.Label(10);

                        _layout.AddSeparateRow(new Padding(7, 7, 7, 0), null, true, false, new [] { label });
                        _layout.AddRow(list);
                    }
                    else
                    {
                        list = _dayLists[day];
                    }

                    _fileLists[path] = list;

                    list.Add(item);
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (_selectedList == null)
            {
                return;
            }

            if (e.Key == Keys.Up)
            {
                if (_selectedList.SelectedRows.First() > 0)
                {
                    _selectedList.SelectRow(_selectedList.SelectedRows.First() - 1);
                }
                else if (_selectedList != _lists.First())
                {
                    _selectedList.UnselectAll();

                    int i = _lists.IndexOf(_selectedList);

                    _selectedList = _lists[i - 1];
                    _selectedList.SelectRow(_selectedList.DataStore.Count() - 1);
                }

                e.Handled = true;
            }
            else if (e.Key == Keys.Down)
            {
                if (_selectedList.SelectedRows.First() < _selectedList.DataStore.Count() - 1)
                {
                    _selectedList.SelectRow(_selectedList.SelectedRows.First() + 1);
                }
                else if (_selectedList != _lists.Last())
                {
                    _selectedList.UnselectAll();

                    int i = _lists.IndexOf(_selectedList);

                    _selectedList = _lists[i + 1];
                    _selectedList.SelectRow(0);
                }

                e.Handled = true;
            }
            else if (e.Key == Keys.Enter)
            {
                string filePath = _selectedList.GetSelectedItem().Path;

                if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                {
                    return;
                }

                RaiseFileSelected(new FileSelectionEventArgs(filePath));
            }
            else if (e.Key == Keys.F5)
            {
                Refresh();
            }
        }

        private void OnFileListCellDoubleClicked(object sender, EventArgs e)
        {
            IModel agents = Models.GetAgents();

            JournalFileListItem item = _selectedList.GetSelectedItem();

            if (string.IsNullOrEmpty(item.Path) || !File.Exists(item.Path) || !agents.ContainsResource(item.Agent))
            {
                return;
            }

            SoftwareAgent agent = agents.GetResource<SoftwareAgent>(item.Agent);

            if (string.IsNullOrEmpty(agent.ExecutableName))
            {
                return;
            }

            ProcessStartInfo process = new ProcessStartInfo();
            process.Arguments = item.Path;
            process.UseShellExecute = true;
            process.WorkingDirectory = Path.GetDirectoryName(item.Path);
            process.FileName = agent.ExecutableName;

            Process.Start(process);
        }

        private void OnFileListSelectionChanged(object sender, EventArgs e)
        {
            if (_selectedList == sender)
            {
                return;
            }

            if (_selectedList != null)
            {
                _selectedList.SelectionChanged -= OnFileListSelectionChanged;
                _selectedList.UnselectAll();
                _selectedList.SelectionChanged += OnFileListSelectionChanged;
            }

            _selectedList = sender as JournalFileList;
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

