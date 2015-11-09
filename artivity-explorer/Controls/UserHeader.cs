using Eto.Forms;
using Eto.Drawing;
using Semiodesk.Trinity;
using Artivity.Model;
using Artivity.Model.ObjectModel;
using ArtivityExplorer.Dialogs.SettingsDialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ArtivityExplorer.Controls
{
    public class UserHeader : StackLayout
    {
        #region Members

        private StackLayout _titleLayout;

        private Label _titleLabel;

        private ImageView _photoBox;

        private Button _preferencesButton;

        #endregion

        #region Constructors

        public UserHeader()
        {
            InitializeComponent();

            Refresh();
        }

        #endregion

        #region Methods

        private void InitializeComponent()
        {
            Orientation = Orientation.Horizontal;
            Padding = new Padding(14, 7);
            Spacing = 7;

            _titleLabel = new Label();
            _titleLabel.TextColor = Color.Parse("#3A4149");
            _titleLabel.Font = SystemFonts.Label(18);

            _titleLayout = new StackLayout();
            _titleLayout.Orientation = Orientation.Vertical;
			_titleLayout.Padding = new Padding (0, 10);
            _titleLayout.Items.Add(_titleLabel);

            _photoBox = new ImageView();
            _photoBox.Size = new Size(75, 75);
            
            _preferencesButton = new Button();
            _preferencesButton.Image = Bitmap.FromResource("preferences");
            _preferencesButton.Width = 32;
            _preferencesButton.Height = 32;
            _preferencesButton.Click += OnPreferencesButtonClick;

            Items.Add(new StackLayoutItem(_photoBox));
            Items.Add(new StackLayoutItem(_titleLayout, HorizontalAlignment.Left, true));
            Items.Add(new StackLayoutItem(_preferencesButton, HorizontalAlignment.Right) { VerticalAlignment = VerticalAlignment.Center });
        }

        public void Refresh()
        {
            Person user = Models.GetAgents().GetResources<Person>().FirstOrDefault();

            if (user == null)
            {
                return;
            }

            if (user.Name == null)
            {
                _titleLabel.Text = " Unkown";
            }
            else
            {
                _titleLabel.Text = " " + user.Name.Split(' ').FirstOrDefault();
            }

            Bitmap photo;

            if (File.Exists(user.Photo))
            {
                photo = new Bitmap(user.Photo);
            }
            else
            {
                photo = Bitmap.FromResource("user");
            }

            _photoBox.Image = photo;
        }

        protected void OnPreferencesButtonClick(object sender, EventArgs e)
        {
            SettingsDialog dialog = new SettingsDialog();
            dialog.ShowModal(this);

            Refresh();
        }

        #endregion
    }
}
