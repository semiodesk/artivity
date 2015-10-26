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
        #region Constructors

        public UserButton()
        {
            Update();
        }

        #endregion

        #region Methods

        protected override void OnClicked(EventArgs e)
        {
            SettingsDialog dialog = new SettingsDialog();
            dialog.Closed += (sender, args) => { Update(); };
            dialog.Run();
        }

        public void Update()
        {
            IModel model = Models.GetAgents();

            Person user = model.GetResources<Person>().FirstOrDefault();

            Label = " " + user.Name.Split(' ').FirstOrDefault();

            if (File.Exists(user.Photo))
            {
                Image = BitmapImage.FromFile(user.Photo).WithSize(30, 30);
            }
            else
            {
                Image = BitmapImage.FromResource("user").WithSize(30, 30);
            }
        }

        #endregion
    }
}

