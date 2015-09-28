using System;
using System.Linq;
using System.IO;
using Xwt;
using Xwt.Drawing;
using Semiodesk.Trinity;
using Artivity.Model;
using Artivity.Model.ObjectModel;

namespace ArtivityExplorer
{
    public class SettingsDialog : Dialog
    {
        #region Members

        private UserSettingsWidget _userSettings;

        private AgentSettingsWidget _agentSettings;

        #endregion

        #region Constructors

        public SettingsDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void InitializeComponent()
        {
            Title = "Preferences";
            Width = 500;
            Height = 500;

            _userSettings = new UserSettingsWidget();
            _agentSettings = new AgentSettingsWidget();

            Notebook notebook = new Notebook();
            notebook.Add(_userSettings, "User");
            notebook.Add(_agentSettings, "Applications");

            Button okButton = new Button();
            okButton.MinWidth = 100;
            okButton.Label = "OK";
            okButton.Clicked += OnOkButtonClicked;

            Button cancelButton = new Button();
            cancelButton.MinWidth = 100;
            cancelButton.Label = "Cancel";
            cancelButton.Clicked += OnCancelButtonClicked;

            HBox buttonLayout = new HBox();
            buttonLayout.Spacing = 7;
            buttonLayout.PackEnd(okButton);
            buttonLayout.PackEnd(cancelButton);

            VBox layout = new VBox();
            layout.PackStart(notebook, true);
            layout.PackStart(buttonLayout);

            Content = layout;
        }

        private void OnOkButtonClicked(object sender, System.EventArgs e)
        {
            _userSettings.Save();
            _agentSettings.Save();

            Close();
        }

        private void OnCancelButtonClicked(object sender, System.EventArgs e)
        {
            Close();
        }

        #endregion
    }
}

