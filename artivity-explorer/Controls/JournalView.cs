using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Semiodesk.Trinity;
using Xwt;
using Xwt.Drawing;
using Artivity.Model;

namespace ArtivityExplorer
{
    public class JournalView : ScrollView
    {
        #region Members

        private readonly VBox _layout = new VBox();

        private List<FileListView> _views = new List<FileListView>();

        private Dictionary<DateTime, FileListView> _dayViews = new Dictionary<DateTime, FileListView>();

        private Dictionary<string, FileListView> _fileViews = new Dictionary<string, FileListView>();

        #endregion

        #region Constructors

        public JournalView()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        protected virtual void InitializeComponent()
        {
            KeyPressed += HandleKeyEvent;

            CanGetFocus = true;
            ExpandVertical = true;

            Content = _layout;
            Content.CanGetFocus = false;
            Content.BackgroundColor = Colors.White;

            Update();
        }

        public void Clear()
        {
            _layout.Clear();

            foreach (FileListView view in _views)
            {
                view.FileSelected -= OnFileListFileSelected;
            }

            _views.Clear();
            _dayViews.Clear();
            _fileViews.Clear();
        }

        public void Update()
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

                SELECT ?startTime ?endTime ?fileUrl WHERE
                {
                       ?activity prov:used ?entity .
                       ?activity prov:startedAtTime ?startTime .
                       ?activity prov:endedAtTime ?endTime .

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

                DateTime startTime = (DateTime)binding["startTime"];
                DateTime endTime = (DateTime)binding["endTime"];
                TimeSpan duration = endTime - startTime;

                if (_fileViews.ContainsKey(path))
                {
                    _fileViews[path].Update(url, path, startTime, duration);
                }
                else
                {
                    DateTime day = startTime.RoundToDay();

                    if (!_dayViews.ContainsKey(day))
                    {
                        FileListView view = new FileListView();
                        view.ExpandVertical = false;
                        view.VerticalScrollPolicy = ScrollPolicy.Never;
                        view.PreviousView = _views.LastOrDefault();
                        view.FileSelected += OnFileListFileSelected;
                        view.SelectionChanged += OnFileListSelectionChanged;

                        _views.Add(view);
                        _dayViews[day] = view;
                        _fileViews[path] = view;

                        Label label = new Label(day.ToString("D"));
                        label.Margin = new WidgetSpacing(10, 10, 10, 0);
                        label.HorizontalPlacement = WidgetPlacement.Start;
                        label.Font = Font.WithWeight(FontWeight.Bold);

                        _layout.PackStart(label, false);
                        _layout.PackStart(view, false);
                    }

                    _dayViews[day].Update(url, path, startTime, duration);
                }
            }
        }

        protected override void OnKeyPressed(KeyEventArgs e)
        {
            HandleKeyEvent(this, e);
        }

        private void HandleKeyEvent(object sender, KeyEventArgs e)
        {
            FileListView selectedView = _views.FirstOrDefault(v => v.SelectedRow > -1);

            if (selectedView == null)
            {
                return;
            }

            if (e.Key == Key.Up)
            {
                if (selectedView.SelectedRow > 0)
                {
                    selectedView.SelectPrevious();
                }
                else if (selectedView != _views.First())
                {
                    selectedView.UnselectAll();

                    int i = _views.IndexOf(selectedView);

                    selectedView = _views[i - 1];
                    selectedView.SelectLast();
                }

                e.Handled = true;
            }
            else if (e.Key == Key.Down)
            {
                if (selectedView.SelectedRow < selectedView.DataSource.RowCount - 1)
                {
                    selectedView.SelectNext();
                }
                else if (selectedView != _views.Last())
                {
                    selectedView.UnselectAll();

                    int i = _views.IndexOf(selectedView);

                    selectedView = _views[i + 1];
                    selectedView.SelectFirst();
                }

                e.Handled = true;
            }
            else if (e.Key == Key.Return || e.Key == Key.NumPadEnter)
            {
                FileSelectionEventArgs args = new FileSelectionEventArgs(selectedView.GetSelectedFile());

                RaiseFileSelected(args);
            }
        }

        private void OnFileListSelectionChanged(object sender, EventArgs e)
        {
            FileListView view = sender as FileListView;

            if (view == null || view.SelectedRow == -1)
                return;

            foreach (FileListView v in _views)
            {
                if (v == view)
                    continue;
                
                v.UnselectAll();
            }
        }

        private void OnFileListFileSelected(object sender, FileSelectionEventArgs e)
        {
            RaiseFileSelected(e);
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

