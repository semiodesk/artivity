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
    public class UserSettingsWidget : HBox
    {
        #region Members

        private IModel _model;

        private Person _user;

        private TextEntry _nameEntry;

        private TextEntry _organizationEntry;

        private TextEntry _emailEntry;

        #endregion

        #region Constructors

        public UserSettingsWidget()
        {
            InitializeModel();
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void InitializeModel()
        {
            IStore store = StoreFactory.CreateStoreFromConfiguration("virt0");

            if (store.ContainsModel(Models.Agents))
            {
                _model = store.GetModel(Models.Agents);
            }
            else
            {
                _model = store.CreateModel(Models.Agents);
            }

            ResourceQuery query = new ResourceQuery(prov.Person);

            _user = _model.GetResources<Person>(query).FirstOrDefault();

            if (_user == null)
            {
                _user = _model.CreateResource<Person>();
                _user.Name = "Unkown";
                _user.Commit();
            }
        }

        private void InitializeComponent()
        {
            Button photoButton = new Button();
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

            _nameEntry = new TextEntry();
            _nameEntry.WidthRequest = 300;
            _nameEntry.Text = _user.Name;

            Label organizationLabel = new Label();
            organizationLabel.Text = "Organization";

            _organizationEntry = new TextEntry();
            _organizationEntry.WidthRequest = 300;
            _organizationEntry.Text = _user.Organization;

            Label emailLabel = new Label();
            emailLabel.Text = "E-Mail";

            _emailEntry = new TextEntry();
            _emailEntry.Text = _user.EmailAddress;

            VBox column1 = new VBox();
            column1.Spacing = 7;
            column1.PackStart(nameLabel);
            column1.PackStart(_nameEntry);
            column1.PackStart(organizationLabel);
            column1.PackStart(_organizationEntry);
            column1.PackStart(emailLabel);
            column1.PackStart(_emailEntry);

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
            if (!string.IsNullOrEmpty(_nameEntry.Text))
            {
                _user.Name = _nameEntry.Text;
            }

            _user.Organization = _organizationEntry.Text;

            if (!string.IsNullOrEmpty(_emailEntry.Text))
            {
                _user.EmailAddress = _emailEntry.Text;
            }

            _user.Commit();
        }

        #endregion
    }
}

