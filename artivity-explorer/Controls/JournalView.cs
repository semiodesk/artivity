using System;
using Semiodesk.Trinity;
using Artivity.Model;
using Xwt;
using System.IO;

namespace ArtivityExplorer
{
    public class JournalView : ListView
    {
        #region Members

        private ListStore _store = new ListStore();

        private DataField<string> _timeField = new DataField<string>();

        private DataField<string> _fileNameField = new DataField<string>();

        private DataField<string> _fileUrlField = new DataField<string>();

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
            // Initialize the list data.
            _store = new ListStore(_timeField, _fileNameField, _fileUrlField);

            // Initialize the list view.
            TextCellView timeView = new TextCellView();
            timeView.TextField = _timeField;

            TextCellView fileView = new TextCellView();
            fileView.TextField = _fileNameField;

            this.CreateColumn<string>(timeView, "Time", Alignment.Start);
            this.CreateColumn<string>(fileView, "File", Alignment.End);

            Update();
        }

        public void Update()
        {
            _store.Clear();

            string queryString = @"
                PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                PREFIX nfo: <http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#>
                PREFIX prov: <http://www.w3.org/ns/prov#>
                PREFIX dces: <http://purl.org/dc/elements/1.1/>

                SELECT ?fileUrl MAX(?t) as ?time WHERE
                {
                    ?activity prov:startedAtTime ?t .
                    ?activity prov:used ?entity .

                    ?entity nfo:fileUrl ?fileUrl .

                    {
                        SELECT DISTINCT ?fileUrl WHERE 
                        {
                            ?activity prov:startedAtTime ?t .
                            ?activity prov:used ?entity .

                            ?entity nfo:fileUrl ?fileUrl .
                        }
                        ORDER BY DESC(?t)
                    }
                }
                GROUP BY ?fileUrl ?time
                ORDER BY DESC(?time)
                LIMIT 20";

            IModel model = Models.GetActivities();

            SparqlQuery query = new SparqlQuery(queryString);
            ISparqlQueryResult result = model.ExecuteQuery(query);

            foreach (BindingSet binding in result.GetBindings())
            {
                string fileUrl = binding["fileUrl"].ToString();

                if (!Uri.IsWellFormedUriString(fileUrl, UriKind.Absolute))
                {
                    continue;
                }

                string fileName = new Uri(fileUrl).AbsolutePath;

                if (!File.Exists(fileName))
                {
                    continue;
                }

                string time = binding["time"].ToString();

                int row = _store.AddRow();

                _store.SetValues(row, _timeField, time, _fileNameField, Path.GetFileName(fileName), _fileUrlField, fileUrl);
            }

            DataSource = _store;
        }

        protected override void OnRowActivated(ListViewRowEventArgs e)
        {
            string fileUrl = _store.GetValue(e.RowIndex, _fileUrlField);

            RaiseFileSelected(fileUrl);
        }

        #endregion

        #region Events

        public FileSelectionEventHandler FileSelected { get; set; }

        private void RaiseFileSelected(string filename)
        {
            if (FileSelected == null) return;

            FileSelected(this, new FileSelectionEventArgs(filename));
        }

        #endregion
    }
}

