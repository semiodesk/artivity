using System;
using System.Linq;
using Xwt;
using Xwt.Drawing;

namespace ArtivityExplorer
{
    public class DialogPage : VBox
    {
        #region Members

        protected Dialog Dialog { get; private set; }

        public string Title
        {
            get { return TitleLabel.Text; }
            set { TitleLabel.Text = value; }
        }

        protected readonly Label TitleLabel = new Label();

        protected readonly VBox ContentHost = new VBox();

        public new Widget Content
        {
            get { return ContentHost.Children.FirstOrDefault(); }
            set
            {
                ContentHost.Clear();
                ContentHost.PackStart(value, true);
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
            MarginTop = 7;
            BackgroundColor = Colors.White;

            TitleLabel.Margin = new WidgetSpacing(28, 28, 28, 7);
            TitleLabel.Font = Font.WithFamily("Helvetica").WithSize(24).WithWeight(FontWeight.Bold);

            ContentHost.Margin = new WidgetSpacing(28, 7, 28, 7);

            Buttons = new DialogNavigationButtons();
            Buttons.OkButton.Clicked += OnOkButtonClicked;
            Buttons.OkButton.Sensitive = false;
            Buttons.OkButton.Visible = false;
            Buttons.CancelButton.Clicked += OnCancelButtonClicked;
            Buttons.CancelButton.SetFocus();
            Buttons.NextButton.Sensitive = false;
            Buttons.NextButton.Clicked += OnNextButtonClicked;
            Buttons.BackButton.Sensitive = false;
            Buttons.BackButton.Clicked += OnBackButtonClicked;

            PackStart(TitleLabel, false);
            PackStart(ContentHost, true);
            PackStart(Buttons, false);
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

