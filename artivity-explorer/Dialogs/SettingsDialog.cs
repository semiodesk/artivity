using System;
using System.Linq;
using System.IO;
using Eto.Forms;
using Eto.Drawing;
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

            TabControl notebook = new TabControl();
            notebook.Pages.Add(new TabPage(_userSettings, new Padding(7)) { Text = "User" });
            notebook.Pages.Add(new TabPage(_agentSettings, new Padding(7)) { Text = "Applications"});

            DialogButtons buttons = new DialogButtons();
            buttons.OkButton.Click += OnOkButtonClicked;
            buttons.CancelButton.Click += OnCancelButtonClicked;

            AbortButton = buttons.CancelButton;
            DefaultButton = buttons.OkButton;

            TableLayout layout = new TableLayout();
            layout.Height = 500;
            layout.Spacing = new Size(0, 7);
            layout.Padding = new Padding(7, 0);
            layout.Rows.Add(new TableRow(notebook) { ScaleHeight = true });
            layout.Rows.Add(new TableRow(buttons));

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

