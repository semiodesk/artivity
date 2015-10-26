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

        private UserSettingsControl _userSettings;

        private AgentSettingsControl _agentSettings;

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

            _userSettings = new UserSettingsControl();
            _agentSettings = new AgentSettingsControl();

            Notebook notebook = new Notebook();
            notebook.Add(_userSettings, "User");
            notebook.Add(_agentSettings, "Applications");

            DialogButtons buttons = new DialogButtons();
            buttons.OkButton.Clicked += OnOkButtonClicked;
            buttons.CancelButton.Clicked += OnCancelButtonClicked;

            VBox layout = new VBox();
            layout.PackStart(notebook, true);
            layout.PackStart(buttons);

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

