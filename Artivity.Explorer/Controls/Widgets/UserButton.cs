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
    public class UserButton : Button
    {
        #region Members

        IModel _model;

        Person _user;

        #endregion

        #region Constructors

        public UserButton()
        {
            Style = ButtonStyle.Borderless;

            InitializeModel();
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

            Update();
        }

        protected override void OnClicked(EventArgs e)
        {
            SettingsDialog dialog = new SettingsDialog();
            dialog.Show();

            Update();
        }

        public void Update()
        {
            ResourceQuery query = new ResourceQuery(prov.Person);

            _user = _model.GetResources<Person>(query).FirstOrDefault();

            if (_user == null)
            {
                _user = _model.CreateResource<Person>();
                _user.Name = "Unkown";
                _user.Commit();
            }

            Label = " " + _user.Name;

            if (File.Exists(_user.Photo))
            {
                Image = BitmapImage.FromFile(_user.Photo).WithSize(30, 30);
            }
            else
            {
                Image = BitmapImage.FromResource("user").WithSize(30, 30);
            }
        }

        #endregion
    }
}

