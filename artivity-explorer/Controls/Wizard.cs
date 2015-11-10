using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto.Forms;
using Eto.Drawing;

namespace Artivity.Explorer.Controls
{
    public class Wizard : Dialog
    {
        #region Members

		protected StackLayout LayoutRoot { get; private set; }

		protected Panel TitleLayout { get; private set; }

        protected StackLayout ButtonLayout { get; private set; }

		[DefaultValue(DialogResult.None)]
        public DialogResult Result { get; private set; }

        private WizardPage _currentPage;

        public WizardPage CurrentPage
        {
            get { return _currentPage; }
            set
            {
                if (value != null)
                {
                    LayoutRoot.Items.RemoveAt(1);
                    LayoutRoot.Items.Insert(1, new StackLayoutItem(value.Content, HorizontalAlignment.Stretch, true));

                    if (string.IsNullOrEmpty(value.Title))
                    {
                        LayoutRoot.Items.RemoveAt(0);
                    }
                    else
                    {
                        TitleLabel.Text = value.Title;
                    }

                    ButtonLayout.Items.Clear();
                    ButtonLayout.Items.Add(new StackLayoutItem(new Panel(), true));

                    foreach(Button b in value.Buttons)
                    {
                        ButtonLayout.Items.Add(new StackLayoutItem(b));
                    }
                }
                else
                {
                    TitleLabel.Visible = false;

                    LayoutRoot.Items.RemoveAt(1);
                }

                _currentPage = value;
            }
        }

        public Button OkButton { get; private set; }

        public Button BackButton { get; private set; }

        public Button NextButton { get; private set; }

        protected Label TitleLabel { get; private set; }

        #endregion

        #region Constructors

        public Wizard() {}

        #endregion

        #region Methods

        protected virtual void InitializeComponent()
        {
            Icon = Icon.FromResource("icon" + Setup.GetIconExtension());

            TitleLabel = new Label();
			TitleLabel.Font = SystemFonts.Label(18);

			TitleLayout = new Panel();
			TitleLayout.Padding = new Padding(24, 14);
			TitleLayout.Content = TitleLabel;

            AbortButton = new Button() { Text = "Cancel", Height = 34, Width = 100 };
            OkButton = new Button() { Text = "Ok", Height = 34, Width = 100 };
            BackButton = new Button() { Text = "< Back", Width = 100, Height = 34 };
            NextButton = new Button() { Text = "Next >", Width = 100, Height = 34 };

            ButtonLayout = new StackLayout();
            ButtonLayout.Orientation = Orientation.Horizontal;
            ButtonLayout.Spacing = 7;
			ButtonLayout.Padding = new Padding(7);
            ButtonLayout.BackgroundColor = Color.Parse("#f0f0f0");

			Padding = new Padding(0);

            LayoutRoot = new StackLayout();
            LayoutRoot.Spacing = 0;
            LayoutRoot.Orientation = Orientation.Vertical;
            LayoutRoot.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            LayoutRoot.VerticalContentAlignment = VerticalAlignment.Stretch;
			LayoutRoot.Items.Add(new StackLayoutItem(TitleLayout, HorizontalAlignment.Left, false));
            LayoutRoot.Items.Add(new StackLayoutItem(new Panel(), HorizontalAlignment.Stretch, true));
            LayoutRoot.Items.Add(new StackLayoutItem(ButtonLayout, HorizontalAlignment.Stretch, false));

            Content = LayoutRoot;
        }

        public void AddButton(Button button)
        {
            ButtonLayout.Items.Add(button);
        }

        public void Close(DialogResult result)
        {
            Result = result;

            Close();
        }

        #endregion
    }
}
