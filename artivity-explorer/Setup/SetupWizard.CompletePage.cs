using System;
using Eto.Forms;
using Eto.Drawing;
using Artivity.Explorer.Controls;

namespace Artivity.Explorer.Dialogs.SetupWizard
{
    public class CompletePage : WizardPage
    {
        private CheckBox _enableLogging;

        private CheckBox _startLogging;

        private CheckBox _setupDatabase;

        public CompletePage(Wizard wizard) : base(wizard) { }

        protected override void InitializeComponent()
        {
            base.InitializeComponent();

            Title = "Complete";

            Label introText = new Label();
			introText.Width = 500;
			introText.Wrap = WrapMode.Word;
            introText.Text = "Finally we need to active our logging service which enables your applications to track your activities.";
            //introText.Font = SystemFonts.Label(10);

            _setupDatabase = new CheckBox();
            _setupDatabase.Enabled = false;
            _setupDatabase.Text = "Setting up the database.";
            //_setupDatabase.Font = SystemFonts.Label(10);

            _enableLogging = new CheckBox();
            _enableLogging.Enabled = false;
            _enableLogging.Text = "Installing logging service into autostart.";
            //_enableLogging.Font = SystemFonts.Label(10);

            _startLogging = new CheckBox();
            _startLogging.Enabled = false;
            _startLogging.Text = "Start logging service.";
            //_startLogging.Font = SystemFonts.Label(10);

            StackLayout layout = new StackLayout();
			layout.Padding = new Padding(24);
			layout.Spacing = 24;
            layout.Items.Add(introText);
            layout.Items.Add(_setupDatabase);
            layout.Items.Add(_enableLogging);
            layout.Items.Add(_startLogging);

            Content = layout;

            Buttons.Add(AbortButton);
            Buttons.Add(OkButton);

            DefaultButton = OkButton;

            BeginSetup();
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

            OkButton.Visible = true;
            OkButton.Enabled = true;
            AbortButton.Visible = false;
            AbortButton.Enabled = false;
        }
    }
}

