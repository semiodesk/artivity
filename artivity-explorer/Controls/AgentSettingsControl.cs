using System;
using System.Text.RegularExpressions;
using Semiodesk.Trinity;
using Artivity.Model;
using Artivity.Model.ObjectModel;
using Xwt;

namespace ArtivityExplorer
{
    public class AgentSettingsControl : ListView
    {
        #region Members

        private ListStore _store;

        public readonly static DataField<Uri> UriField = new DataField<Uri>();

        public readonly static DataField<bool> EnabledField = new DataField<bool>();

        public readonly static DataField<string> NameField = new DataField<string>();

        public readonly static DataField<string> ColourField = new DataField<string>();

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
            _store = new ListStore(UriField, EnabledField, NameField, ColourField);

            IModel model = Models.GetAgents();

            foreach (SoftwareAgent agent in model.GetResources<SoftwareAgent>())
            {
                if (agent.Name == "" || agent.ColourCode == null)
                {
                    // We found a corrupted entry in the database.
                    model.DeleteResource(agent);

                    continue;
                }

                int row = _store.AddRow();

                _store.SetValues(row, UriField, agent.Uri, EnabledField, agent.IsCaptureEnabled, NameField, agent.Name, ColourField, agent.ColourCode);
            }

            // Initialize the list view.
            Columns.AddCheckBoxColumn(EnabledField, "", Alignment.End);
            Columns.AddTextColumn(NameField, "Name", Alignment.Start);
            Columns.AddTextColumn(ColourField, "Colour", Alignment.End, true);
            DataSource = _store;
        }

        public void Save()
        {
            IModel model = Models.GetAgents();

            for (int i = 0; i < _store.RowCount; i++)
            {
                string colour = _store.GetValue(i, ColourField);

                Regex expression = new Regex("^#([A-Fa-f0-9]{6})$");

                if (string.IsNullOrEmpty(colour) || !expression.IsMatch(colour))
                {
                    continue;
                }

                Uri uri = _store.GetValue(i, UriField);

                SoftwareAgent agent = model.GetResource<SoftwareAgent>(uri);
                agent.ColourCode = colour;
                agent.Commit();
            }
        }

        #endregion
    }
}

