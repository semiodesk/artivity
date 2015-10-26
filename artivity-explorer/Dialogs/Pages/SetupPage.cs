using System;
using Xwt;

namespace ArtivityExplorer
{
    public class SetupPage : DialogPage
    {
        private CheckBox _enableLogging;

        private CheckBox _startLogging;

        private CheckBox _setupDatabase;

        public SetupPage(Dialog dialog) : base(dialog) {}

        protected override void InitializeComponent()
        {
            base.InitializeComponent();

            Title = "Ready";

            Buttons.HorizontalPlacement = WidgetPlacement.Center;
            Buttons.ExpandHorizontal = false;
            Buttons.BackButton.Visible = false;
            Buttons.NextButton.Visible = false;
            Buttons.OkButton.Visible = false;
            Buttons.OkButton.Sensitive = false;
            Buttons.OkButton.MinWidth = 120;
            Buttons.CancelButton.Visible = true;
            Buttons.CancelButton.Sensitive = true;
            Buttons.CancelButton.MinWidth = 120;

            Label introText = new Label("Finally we need to active our logging service which enables \nyour applications to track your activities.");
            introText.ExpandVertical = true;
            introText.Ellipsize = EllipsizeMode.None;
            introText.Margin = new WidgetSpacing(0, 0, 0, 28);

            _setupDatabase = new CheckBox();
            _setupDatabase.Sensitive = false;
            _setupDatabase.Label = "Setting up the database.";

            _enableLogging = new CheckBox();
            _enableLogging.Sensitive = false;
            _enableLogging.Label = "Installing logging service into autostart.";

            _startLogging = new CheckBox();
            _startLogging.Sensitive = false;
            _startLogging.Label = "Start logging service.";

            VBox layout = new VBox();
            layout.PackStart(introText);
            layout.PackStart(_setupDatabase);
            layout.PackStart(_enableLogging);
            layout.PackStart(_startLogging);

            Content = layout;
        }

        public void BeginSetup()
        {
            _setupDatabase.State = Setup.InstallModels() ? CheckBoxState.On : CheckBoxState.Off;

            if (!Setup.HasApiDaemonAutostart())
            {
                _enableLogging.State = Setup.InstallApiDaemonAutostart() ? CheckBoxState.On : CheckBoxState.Off;
                _startLogging.State = Setup.TryStartApiDaemon() ? CheckBoxState.On : CheckBoxState.Off;
            }
            else
            {
                _enableLogging.State = CheckBoxState.On;
                _startLogging.State = CheckBoxState.On;
            }

            _setupDatabase.Sensitive = _setupDatabase.State == CheckBoxState.On;
            _enableLogging.Sensitive = _enableLogging.State == CheckBoxState.On;
            _startLogging.Sensitive = _startLogging.State == CheckBoxState.On;

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

