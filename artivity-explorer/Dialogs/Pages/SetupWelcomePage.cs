using System;
using Eto.Forms;

namespace ArtivityExplorer
{
    public class SetupWelcomePage : DialogPage
    {
        private UserSettingsControl _userSettings;

        private CheckBox _agreePrivacy;

        public SetupWelcomePage(Dialog dialog) : base(dialog) {}

        protected override void InitializeComponent()
        {
            base.InitializeComponent();

            Title = "Welcome";

            Label introText = new Label();
            introText.Text = "This is the first time you start Artivity. Before you can start \nusing the app, we need you to set up your user account.";

            _userSettings = new UserSettingsControl();
            _userSettings.NameBox.Focus();
            _userSettings.NameBox.TextChanged += Validate;

            _agreePrivacy = new CheckBox();
            _agreePrivacy.Text = "I agree to the privacy statement. (TODO: Link to document)";
            _agreePrivacy.CheckedChanged += Validate;

            StackLayout layout = new StackLayout();
            layout.Spacing = 24;
            layout.Items.Add(new StackLayoutItem(introText, false));
            layout.Items.Add(new StackLayoutItem(_userSettings, false));
            layout.Items.Add(new StackLayoutItem(_agreePrivacy, false));

            Content = layout;

            Validate(this, new EventArgs());

            Buttons.BackButton.Visible = false;

            Dialog.DefaultButton = Buttons.NextButton;
            Dialog.AbortButton = Buttons.CancelButton;
        }
            
        private void Validate(object sender, EventArgs e)
        {
            Buttons.NextButton.Enabled =
                !string.IsNullOrEmpty(_userSettings.NameBox.Text) &&
                !string.IsNullOrEmpty(_userSettings.EmailBox.Text) &&
                _agreePrivacy.Checked == true;
        }

        protected override void OnNextButtonClicked(object sender, EventArgs e)
        {
            _userSettings.Save();

            SetupReadyPage page = new SetupReadyPage(Dialog);

            Dialog.Content = page;

            page.BeginSetup();
        }
    }
}

