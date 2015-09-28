using System;
using Semiodesk.Trinity;
using Xwt;
using Artivity.Model;
using Artivity.Model.ObjectModel;
using System.Text.RegularExpressions;

namespace ArtivityExplorer
{
    public class AgentSettingsWidget : VBox
    {
        #region Members

        private IModel _model;

        private ListStore _store;

        private ListView _view;

        private DataField<Uri> _uriField = new DataField<Uri>();

        private DataField<string> _nameField = new DataField<string>();

        private DataField<bool> _enabledField = new DataField<bool>();

        private DataField<string> _colourField = new DataField<string>();

        #endregion

        #region Constructors

        public AgentSettingsWidget()
        {
            InitializeModel();
            InitializeAgents();
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void InitializeModel()
        {
            IStore store = StoreFactory.CreateStoreFromConfiguration("virt0");

            if (store.ContainsModel(Models.Agents))
            {
                _model = store.GetModel(Models.Agents);
            }
            else
            {
                _model = store.CreateModel(Models.Agents);
            }
        }

        private void InitializeAgents()
        {
            SetupAgent("application://inkscape.desktop/", "Inkscape", "#EE204E");
            SetupAgent("application://krita.desktop/", "Krita", "#926EAE");
            SetupAgent("application://chromium-browser.desktop/", "Chromium Browser", "#1F75FE");
            SetupAgent("application://firefox-browser.desktop/", "Firefox Browser", "#1F75FE");
        }

        private void SetupAgent(string uri, string name, string colour)
        {
            if (!_model.ContainsResource(new UriRef(uri)))
            {
                SoftwareAgent agent = _model.CreateResource<SoftwareAgent>(new UriRef(uri));
                agent.Name = name;
                agent.IsCaptureEnabled = false;
                agent.Commit();
            }
            else
            {
                bool modified = false;

                SoftwareAgent agent = _model.GetResource<SoftwareAgent>(new UriRef(uri));

                if(!agent.HasProperty(rdf.type, prov.SoftwareAgent))
                {
                    agent.AddProperty(rdf.type, prov.SoftwareAgent);

                    modified = true;
                }

                if (agent.Name != name)
                {
                    agent.Name = name;

                    modified = true;
                }

                if (string.IsNullOrEmpty(agent.ColourCode))
                {
                    agent.ColourCode = colour;

                    modified = true;
                }

                if (modified)
                {
                    agent.Commit();
                }
            }
        }

        private void InitializeComponent()
        {
            Margin = new WidgetSpacing(6, 7, 6, 7);

            // Initialize the list data.
            _store = new ListStore(_uriField, _enabledField, _nameField, _colourField);

            ResourceQuery query = new ResourceQuery(prov.SoftwareAgent);

            foreach (SoftwareAgent agent in _model.GetResources<SoftwareAgent>(query, true))
            {
                int row = _store.AddRow();

                _store.SetValues(row, _uriField, agent.Uri, _enabledField, agent.IsCaptureEnabled, _nameField, agent.Name, _colourField, agent.ColourCode);
            }

            // Initialize the list view.
            CheckBoxCellView enabledView = new CheckBoxCellView();
            enabledView.ActiveField = _enabledField;

            TextCellView nameView = new TextCellView();
            nameView.MarkupField = _nameField;

            TextCellView colourView = new TextCellView();
            colourView.Editable = true;
            colourView.TextField = _colourField;

            _view = new ListView();
            _view.Columns.Add(CreateColumn<bool>(enabledView, "Enabled", Alignment.End));
            _view.Columns.Add(CreateColumn<string>(nameView, "Name", Alignment.Start));
            _view.Columns.Add(CreateColumn<string>(colourView, "Colour", Alignment.End));
            _view.DataSource = _store;

            PackStart(_view, true);
        }

        private ListViewColumn CreateColumn<T>(CellView view, string title, Alignment alignment, bool canResize = false)
        {
            ListViewColumn column = new ListViewColumn (title, view);
            column.Alignment = alignment;
            column.CanResize = canResize;

            return column;
        }

        public void Save()
        {
            for (int i = 0; i < _store.RowCount; i++)
            {
                string colour = _store.GetValue(i, _colourField);

                Regex expression = new Regex("^#([A-Fa-f0-9]{6})$");

                if (!expression.IsMatch(colour))
                    continue;

                Uri uri = _store.GetValue(i, _uriField);

                SoftwareAgent agent = _model.GetResource<SoftwareAgent>(uri);
                agent.ColourCode = colour;
                agent.Commit();
            }
        }

        #endregion
    }
}

