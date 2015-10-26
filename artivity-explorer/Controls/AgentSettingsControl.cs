using System;
using System.Text.RegularExpressions;
using Semiodesk.Trinity;
using Artivity.Model;
using Artivity.Model.ObjectModel;
using Xwt;

namespace ArtivityExplorer
{
    public class AgentSettingsControl : VBox
    {
        #region Members

        private ListStore _store;

        private ListView _view;

        private DataField<Uri> _uriField = new DataField<Uri>();

        private DataField<string> _nameField = new DataField<string>();

        private DataField<bool> _enabledField = new DataField<bool>();

        private DataField<string> _colourField = new DataField<string>();

        #endregion

        #region Constructors

        public AgentSettingsControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void InitializeComponent()
        {
            Margin = new WidgetSpacing(6, 7, 6, 7);

            // Initialize the list data.
            _store = new ListStore(_uriField, _enabledField, _nameField, _colourField);

            IModel model = Models.GetAgents();

            foreach (SoftwareAgent agent in model.GetResources<SoftwareAgent>())
            {
                if (agent.Name == "" || agent.ColourCode == null)
                {
                    continue;
                }

                int row = _store.AddRow();

                _store.SetValues(row, _uriField, agent.Uri, _enabledField, agent.IsCaptureEnabled, _nameField, agent.Name, _colourField, agent.ColourCode);
            }

            // Initialize the list view.
            CheckBoxCellView enabledView = new CheckBoxCellView();
            enabledView.ActiveField = _enabledField;

            TextCellView nameView = new TextCellView();
            nameView.TextField = _nameField;

            TextCellView colourView = new TextCellView();
            colourView.Editable = true;
            colourView.TextField = _colourField;

            _view = new ListView();
            _view.CreateColumn<bool>(enabledView, "Enabled", Alignment.End);
            _view.CreateColumn<string>(nameView, "Name", Alignment.Start);
            _view.CreateColumn<string>(colourView, "Colour", Alignment.End);
            _view.DataSource = _store;

            PackStart(_view, true);
        }

        public void Save()
        {
            IModel model = Models.GetAgents();

            for (int i = 0; i < _store.RowCount; i++)
            {
                string colour = _store.GetValue(i, _colourField);

                Regex expression = new Regex("^#([A-Fa-f0-9]{6})$");

                if (string.IsNullOrEmpty(colour) || !expression.IsMatch(colour))
                    continue;

                Uri uri = _store.GetValue(i, _uriField);

                SoftwareAgent agent = model.GetResource<SoftwareAgent>(uri);
                agent.ColourCode = colour;
                agent.Commit();
            }
        }

        #endregion
    }
}

