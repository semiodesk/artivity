using System;
using System.Linq;
using System.IO;
using Xwt;
using Xwt.Drawing;
using Semiodesk.Trinity;
using Artivity.Model;
using Artivity.Model.ObjectModel;

namespace ArtivityExplorer
{
    public class UserSettingsControl : HBox
    {
        #region Members

        private Person _user;

        public TextEntry NameEntry;

        public TextEntry OrganizationEntry;

        public TextEntry EmailEntry;

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

            ResourceQuery query = new ResourceQuery(prov.Person);

            _user = model.GetResources<Person>(query).FirstOrDefault();

            if (_user == null)
            {
                _user = model.CreateResource<Person>();
                _user.Name = "Unkown";
                _user.Commit();
            }
        }

        private void InitializeComponent()
        {
            Button photoButton = new Button();
            photoButton.Style = ButtonStyle.Flat;
            photoButton.ImagePosition = ContentPosition.Top | ContentPosition.Left;
            photoButton.ExpandHorizontal = false;
            photoButton.ExpandVertical = false;
            photoButton.Clicked += OnPhotoButtonClicked;

            if (File.Exists(_user.Photo))
            {
                photoButton.Image = BitmapImage.FromFile(_user.Photo);
            }
            else
            {
                photoButton.Image = BitmapImage.FromResource("user");
            }

            VBox column0 = new VBox();
            column0.PackStart(photoButton);

            Label nameLabel = new Label();
            nameLabel.Text = "Name";

            NameEntry = new TextEntry();
            NameEntry.WidthRequest = 300;
            NameEntry.Text = _user.Name;

            Label organizationLabel = new Label();
            organizationLabel.Text = "Organization";

            OrganizationEntry = new TextEntry();
            OrganizationEntry.WidthRequest = 300;
            OrganizationEntry.Text = _user.Organization;

            Label emailLabel = new Label();
            emailLabel.Text = "E-Mail";

            EmailEntry = new TextEntry();
            EmailEntry.Text = _user.EmailAddress;

            VBox column1 = new VBox();
            column1.Spacing = 7;
            column1.PackStart(nameLabel);
            column1.PackStart(NameEntry);
            column1.PackStart(organizationLabel);
            column1.PackStart(OrganizationEntry);
            column1.PackStart(emailLabel);
            column1.PackStart(EmailEntry);

            Margin = 7;
            Spacing = 14;
            PackStart(column0);
            PackStart(column1);
        }

        private void OnPhotoButtonClicked(object sender, System.EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filters.Add(new FileDialogFilter("Images", "*.png"));
            openDialog.Run();

            if (!string.IsNullOrEmpty(openDialog.FileName))
            {
                _user.Photo = openDialog.FileName;

                Image avatar = BitmapImage.FromFile(_user.Photo);

                Button avatarButton = sender as Button;
                avatarButton.Image = avatar;
            }
        }

        public void Save()
        {
            if (!string.IsNullOrEmpty(NameEntry.Text))
            {
                _user.Name = NameEntry.Text;
            }

            _user.Organization = OrganizationEntry.Text;

            if (!string.IsNullOrEmpty(EmailEntry.Text))
            {
                _user.EmailAddress = EmailEntry.Text;
            }

            _user.Commit();
        }

        #endregion
    }
}

