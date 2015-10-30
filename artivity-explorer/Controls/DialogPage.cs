using System;
using System.Linq;
using Eto;
using Eto.Forms;
using Eto.Drawing;

namespace ArtivityExplorer
{
    public class DialogPage : StackLayout
    {
        #region Members

        protected Dialog Dialog { get; private set; }

        public string Title
        {
            get { return TitleLabel.Text; }
            set { TitleLabel.Text = value; }
        }

        protected readonly Label TitleLabel = new Label();

        protected readonly StackLayout ContentHost = new StackLayout();

        public new Control Content
        {
            get { return ContentHost.Children.FirstOrDefault(); }
            set
            {
                ContentHost.Items.Clear();
                ContentHost.Items.Add(new StackLayoutItem(value, true));
            }
        }

        public DialogNavigationButtons Buttons { get; private set; }

        public DialogPage NextPage { get; set; }

        public DialogPage BackPage { get; set; }

        #endregion

        #region Constructors

        public DialogPage(Dialog dialog)
        {
            Dialog = dialog;

            InitializeComponent();
        }

        #endregion

        #region Methods

        protected virtual void InitializeComponent()
        {
            Height = Dialog.Height;

            BackgroundColor = Colors.White;

            TitleLabel.Font = SystemFonts.Label(24);

            Orientation = Orientation.Vertical;
            Spacing = 14;
            Padding = new Padding(24);

            Buttons = new DialogNavigationButtons();
            Buttons.OkButton.Click += OnOkButtonClicked;
            Buttons.OkButton.Enabled = false;
            Buttons.OkButton.Visible = false;
            Buttons.CancelButton.Click += OnCancelButtonClicked;
            Buttons.CancelButton.Focus();
            Buttons.NextButton.Enabled = false;
            Buttons.NextButton.Click += OnNextButtonClicked;
            Buttons.BackButton.Enabled = false;
            Buttons.BackButton.Click += OnBackButtonClicked;

            Items.Add(new StackLayoutItem(TitleLabel, HorizontalAlignment.Left));
            Items.Add(new StackLayoutItem(ContentHost, HorizontalAlignment.Stretch, true));
            Items.Add(new StackLayoutItem(Buttons, HorizontalAlignment.Right));
        }

        protected virtual void OnOkButtonClicked(object sender, EventArgs e)
        {
            Dialog.Close();
        }

        protected virtual void OnCancelButtonClicked(object sender, EventArgs e)
        {
            Dialog.Close();
        }

        protected virtual void OnNextButtonClicked(object sender, EventArgs e)
        {
        }

        protected virtual void OnBackButtonClicked(object sender, EventArgs e)
        {
        }

        #endregion
    }
}

