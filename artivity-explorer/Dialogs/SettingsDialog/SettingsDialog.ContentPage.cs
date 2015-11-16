using Artivity.Explorer.Controls;
using Eto.Drawing;
using Eto.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Artivity.Explorer.Dialogs.SettingsDialog
{
    public class ContentPage : WizardPage
    {
        #region Members

        private UserSettingsControl _userSettings = new UserSettingsControl();

        private AgentSettingsControl _agentSettings = new AgentSettingsControl();

        private DatabaseSettingsControl _databaseSettings = new DatabaseSettingsControl();

        #endregion

        #region Constructors

        public ContentPage(Wizard wizard) : base(wizard) { }

        #endregion

        #region Methods

        protected override void InitializeComponent()
        {
            base.InitializeComponent();

            TabControl tabs = new TabControl();
            tabs.Pages.Add(new TabPage(_userSettings, new Padding(7)) { Text = "User" });
            tabs.Pages.Add(new TabPage(_agentSettings, new Padding(7)) { Text = "Applications" });
            tabs.Pages.Add(new TabPage(_databaseSettings, new Padding(7)) { Text = "Database" });

            Content = tabs;

            Buttons.Add(AbortButton);
            Buttons.Add(OkButton);

            OkButton.Enabled = true;
            AbortButton.Enabled = true;

            DefaultButton = OkButton;

            Wizard.Title = "Preferences";
        }

        protected override void OnOkButtonClicked(object sender, EventArgs e)
        {
            _userSettings.Save();
            _agentSettings.Save();

            base.OnOkButtonClicked(sender, e);
        }

        #endregion
    }
}
