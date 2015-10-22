using System;
using Xwt;

namespace ArtivityExplorer
{
    public class SetupPage : DialogPage
    {
        private CheckBox _enableLogging;

        private CheckBox _startLogging;

        public SetupPage(Dialog dialog) : base(dialog) {}

        protected override void InitializeComponent()
        {
            base.InitializeComponent();

            Title = "Installing";

            Buttons.BackButton.Visible = false;
            Buttons.NextButton.Visible = false;
            Buttons.OkButton.Visible = false;
            Buttons.OkButton.Sensitive = false;
            Buttons.CancelButton.Visible = true;
            Buttons.CancelButton.Sensitive = true;

            Label introText = new Label("Finally we need to active our logging service which enables \nyour applications to track your activities.");
            introText.ExpandVertical = true;
            introText.Ellipsize = EllipsizeMode.None;
            introText.Margin = new WidgetSpacing(0, 0, 0, 28);

            _enableLogging = new CheckBox();
            _enableLogging.Sensitive = false;
            _enableLogging.Active = true;
            _enableLogging.Label = "Enable autostart for logging service.";

            _startLogging = new CheckBox();
            _startLogging.Sensitive = false;
            _startLogging.Active = true;
            _startLogging.Label = "Start logging service.";

            VBox layout = new VBox();
            layout.PackStart(introText);
            layout.PackStart(_enableLogging);
            layout.PackStart(_startLogging);

            Content = layout;
        }

        public void BeginSetup()
        {
            if (!SetupHelper.HasApiDaemonAutostart())
            {
                _enableLogging.State = SetupHelper.InstallApiDaemonAutostart() ? CheckBoxState.On : CheckBoxState.Off;
                _startLogging.State = SetupHelper.TryStartApiDaemon() ? CheckBoxState.On : CheckBoxState.Off;
            }
            else
            {
                _enableLogging.State = CheckBoxState.On;
                _startLogging.State = CheckBoxState.On;
            }

            Buttons.OkButton.Visible = true;
            Buttons.OkButton.Sensitive = true;
            Buttons.CancelButton.Visible = false;
            Buttons.CancelButton.Sensitive = false;

            SetupDialog setup = Dialog as SetupDialog;

            if (setup != null)
            {
                setup.Success = true;
            }
        }

        protected override void OnOkButtonClicked(object sender, EventArgs e)
        {
            Dialog.Close();
        }
    }
}

