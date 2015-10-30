using System;
using Eto.Forms;

namespace ArtivityExplorer
{
    public class SetupReadyPage : DialogPage
    {
        private CheckBox _enableLogging;

        private CheckBox _startLogging;

        private CheckBox _setupDatabase;

        public SetupReadyPage(Dialog dialog) : base(dialog) {}

        protected override void InitializeComponent()
        {
            base.InitializeComponent();

            Title = "Ready.";

            Label introText = new Label();
            introText.Text = "Finally we need to active our logging service which enables \nyour applications to track your activities.\n";

            _setupDatabase = new CheckBox();
            _setupDatabase.Enabled = false;
            _setupDatabase.Text = "Setting up the database.";

            _enableLogging = new CheckBox();
            _enableLogging.Enabled = false;
            _enableLogging.Text = "Installing logging service into autostart.";

            _startLogging = new CheckBox();
            _startLogging.Enabled = false;
            _startLogging.Text = "Start logging service.";

            StackLayout layout = new StackLayout();
            layout.Items.Add(introText);
            layout.Items.Add(_setupDatabase);
            layout.Items.Add(_enableLogging);
            layout.Items.Add(_startLogging);

            Content = layout;

            Buttons.BackButton.Visible = false;
            Buttons.NextButton.Visible = false;
            Buttons.OkButton.Visible = false;
            Buttons.OkButton.Enabled = false;
            Buttons.CancelButton.Visible = true;
            Buttons.CancelButton.Enabled = true;

            Dialog.DefaultButton = Buttons.OkButton;
            Dialog.AbortButton = Buttons.CancelButton;
        }

        public void BeginSetup()
        {
            _setupDatabase.Checked = Setup.InstallModels() ? true : false;

            if (!Setup.HasApiDaemonAutostart())
            {
                _enableLogging.Checked = Setup.InstallApiDaemonAutostart() ? true : false;
                _startLogging.Checked = Setup.TryStartApiDaemon() ? true : false;
            }
            else
            {
                _enableLogging.Checked = true;
                _startLogging.Checked = true;
            }

            _setupDatabase.Enabled = _setupDatabase.Checked == true;
            _enableLogging.Enabled = _enableLogging.Checked == true;
            _startLogging.Enabled = _startLogging.Checked == true;

            Buttons.OkButton.Visible = true;
            Buttons.OkButton.Enabled = true;
            Buttons.CancelButton.Visible = false;
            Buttons.CancelButton.Enabled = false;

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

