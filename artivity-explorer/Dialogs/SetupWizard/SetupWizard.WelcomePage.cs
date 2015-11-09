using System;
using Eto.Forms;
using Eto.Drawing;
using ArtivityExplorer.Controls;

namespace ArtivityExplorer.Dialogs.SetupWizard
{
    public class WelcomePage : WizardPage
    {
        private UserSettingsControl _userSettings;

        private CheckBox _agreePrivacy;

        public WelcomePage(Wizard wizard) : base(wizard) { }

        protected override void InitializeComponent()
        {
            base.InitializeComponent();

            Title = "Welcome";

            Label introText = new Label();
            introText.Wrap = WrapMode.Word;
            introText.Text = "This is the first time you start Artivity. Before you can start using the app, we need you to set up your user account.";

            _userSettings = new UserSettingsControl();
            _userSettings.NameBox.Focus();
            _userSettings.NameBox.TextChanged += Validate;

            _agreePrivacy = new CheckBox();
            _agreePrivacy.Text = "I agree to the privacy statement. (TODO: Link to document)";
            _agreePrivacy.CheckedChanged += Validate;

            StackLayout layout = new StackLayout();
			layout.Padding = new Padding(24, 0);
			layout.Spacing = 24;
			layout.Items.Add(new StackLayoutItem(introText, false));
            layout.Items.Add(new StackLayoutItem(_userSettings, false));
			layout.Items.Add(new StackLayoutItem(_agreePrivacy, false));

            Content = layout;

            Buttons.Add(AbortButton);
            Buttons.Add(NextButton);

            AbortButton.Enabled = true;
            NextButton.Enabled = true;
            DefaultButton = NextButton;

			Validate(this, new EventArgs());
        }
            
        private void Validate(object sender, EventArgs e)
        {
            NextButton.Enabled =
                !string.IsNullOrEmpty(_userSettings.NameBox.Text) &&
                !string.IsNullOrEmpty(_userSettings.EmailBox.Text) &&
                _agreePrivacy.Checked == true;
        }

        protected override void OnNextButtonClicked(object sender, EventArgs e)
        {
            _userSettings.Save();

            Wizard.CurrentPage = new CompletePage(Wizard);
        }
    }
}

