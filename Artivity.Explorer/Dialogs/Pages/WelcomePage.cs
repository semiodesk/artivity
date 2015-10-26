using System;
using Xwt;

namespace ArtivityExplorer
{
    public class WelcomePage : DialogPage
    {
        private UserSettingsControl _userSettings;

        private CheckBox _agreePrivacy;

        public WelcomePage(Dialog dialog) : base(dialog) {}

        protected override void InitializeComponent()
        {
            base.InitializeComponent();

            Title = "Welcome";

            Buttons.BackButton.Visible = false;

            Label introText = new Label("This is the first time you start Artivity. Before you can start \nusing the app, we need you to set up your user account.");
            introText.ExpandVertical = true;
            introText.Ellipsize = EllipsizeMode.None;

            _userSettings = new UserSettingsControl();
            _userSettings.Margin = new WidgetSpacing(0, 28, 0, 0);
            _userSettings.NameEntry.SetFocus();
            _userSettings.NameEntry.Changed += Validate;

            _agreePrivacy = new CheckBox();
            _agreePrivacy.Margin = new WidgetSpacing(0, 28, 0, 0);
            _agreePrivacy.Label = "I agree to the privacy statement. (TODO: Link to document)";
            _agreePrivacy.Toggled += Validate;

            VBox layout = new VBox();
            layout.PackStart(introText, false);
            layout.PackStart(_userSettings, false);
            layout.PackStart(_agreePrivacy, false);

            Content = layout;

            Validate(null, null);
        }
            
        private void Validate(object sender, EventArgs e)
        {
            Buttons.NextButton.Sensitive =
                !string.IsNullOrEmpty(_userSettings.NameEntry.Text) &&
                !string.IsNullOrEmpty(_userSettings.EmailEntry.Text) &&
                _agreePrivacy.State == CheckBoxState.On;
        }

        protected override void OnNextButtonClicked(object sender, EventArgs e)
        {
            _userSettings.Save();

            SetupPage page = new SetupPage(Dialog);

            Dialog.Content = page;

            page.BeginSetup();
        }
    }
}

