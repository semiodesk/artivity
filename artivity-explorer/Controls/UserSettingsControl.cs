using System;
using System.Linq;
using System.IO;
using Eto.Forms;
using Eto.Drawing;
using Semiodesk.Trinity;
using Artivity.Model;
using Artivity.Model.ObjectModel;

namespace Artivity.Explorer
{
    public class UserSettingsControl : StackLayout
    {
        #region Members

        private Person _user;

        public TextBox NameBox;

        public TextBox OrganizationBox;

        public MaskedTextBox EmailBox;

        #endregion

        #region Constructors

        public UserSettingsControl()
        {
            InitializeModel();
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void InitializeModel()
        {
            IModel model = Models.GetAgents();

            _user = model.GetResources<Person>().FirstOrDefault();

            if (_user == null)
            {
                _user = model.CreateResource<Person>();
                _user.Name = "Unkown";
                _user.Commit();
            }
        }

        private void InitializeComponent()
        {
            Spacing = 14;

            Button photoButton = new Button();
            photoButton.Width = 93;
            photoButton.Height = 93;
            photoButton.ImagePosition = ButtonImagePosition.Overlay;
            photoButton.Click += OnPhotoButtonClicked;

            if (File.Exists(_user.Photo))
            {
                photoButton.Image = new Bitmap(_user.Photo);
            }
            else
            {
                photoButton.Image = Bitmap.FromResource("user");
            }

            StackLayout column0 = new StackLayout();
            column0.Items.Add(photoButton);

            Label nameLabel = new Label();
            nameLabel.Text = "Name";

            NameBox = new TextBox();
            NameBox.Width = 250;
            NameBox.Text = _user.Name;

            Label organizationLabel = new Label();
            organizationLabel.Text = "Organization";

            OrganizationBox = new TextBox();
            OrganizationBox.Width = 250;
            OrganizationBox.Text = _user.Organization;

            Label emailLabel = new Label();
            emailLabel.Text = "E-Mail";

            EmailBox = new MaskedTextBox();
            EmailBox.Width = 250;
            EmailBox.Text = _user.EmailAddress;

            StackLayout column1 = new StackLayout();
            column1.Spacing = 7;
            column1.Items.Add(nameLabel);
            column1.Items.Add(NameBox);
            column1.Items.Add(organizationLabel);
            column1.Items.Add(OrganizationBox);
            column1.Items.Add(emailLabel);
            column1.Items.Add(EmailBox);

            Orientation = Orientation.Horizontal;
            Spacing = 14;

            Items.Add(column0);
            Items.Add(column1);
        }

        private void OnPhotoButtonClicked(object sender, System.EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filters.Add(new FileDialogFilter("Images", "*.png"));
            openDialog.ShowDialog(this);

            if (!string.IsNullOrEmpty(openDialog.FileName))
            {
                try
                {
                    string sourceFile = openDialog.FileName;
                    string targetFile = Path.Combine(Setup.GetAppDataFolder(), Path.GetFileName(openDialog.FileName));

                    File.Copy(sourceFile, targetFile, true);

                    _user.Photo = targetFile;
                    _user.Commit();

                    Image avatar = new Bitmap(_user.Photo);

                    Button avatarButton = sender as Button;
                    avatarButton.Image = new Bitmap(avatar, 75, 75, ImageInterpolation.High);
                }
                catch(IOException ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        public void Save()
        {
            if (!string.IsNullOrEmpty(NameBox.Text))
            {
                _user.Name = NameBox.Text;
            }

            _user.Organization = OrganizationBox.Text;

            if (!string.IsNullOrEmpty(EmailBox.Text))
            {
                _user.EmailAddress = EmailBox.Text;
            }

            _user.Commit();
        }

        #endregion
    }
}

