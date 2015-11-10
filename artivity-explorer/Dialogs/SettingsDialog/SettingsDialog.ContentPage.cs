using Artivity.Explorer.Controls;
using Eto.Drawing;
using Eto.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artivity.Explorer.Dialogs.SettingsDialog
{
    public class ContentPage : WizardPage
    {
        #region Members

        private UserSettingsControl _userSettings;

        private AgentSettingsControl _agentSettings;

        #endregion

        #region Constructors

        public ContentPage(Wizard wizard) : base(wizard) { }

        #endregion

        #region Methods

        protected override void InitializeComponent()
        {
            base.InitializeComponent();

            _userSettings = new UserSettingsControl();
            _agentSettings = new AgentSettingsControl();

            TabControl tabs = new TabControl();
            tabs.Pages.Add(new TabPage(_userSettings, new Padding(7)) { Text = "User" });
            tabs.Pages.Add(new TabPage(_agentSettings, new Padding(7)) { Text = "Applications" });

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
