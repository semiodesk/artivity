using System;
using System.Linq;
using System.IO;
using Eto.Forms;
using Eto.Drawing;
using Semiodesk.Trinity;
using Artivity.Model;
using Artivity.Model.ObjectModel;

namespace ArtivityExplorer
{
    public class SettingToolItem : ButtonToolItem
    {
        #region Constructors

        public SettingToolItem()
        {
            Refresh();

            Click += OnClick;
        }

        #endregion

        #region Methods

        protected void OnClick(object sender, EventArgs e)
        {
            SettingsDialog dialog = new SettingsDialog();
            dialog.ShowModal();

            Refresh();
        }

        public void Refresh()
        {
            Person user = Models.GetAgents().GetResources<Person>().FirstOrDefault();

            if (user == null)
            {
                return;
            }

            Text = " " + user.Name.Split(' ').FirstOrDefault();

            Bitmap photo;

            if (File.Exists(user.Photo))
            {
                photo = new Bitmap(user.Photo);
            }
            else
            {
                photo = Bitmap.FromResource("user");
            }

            Image = new Bitmap(photo, 30, 30, ImageInterpolation.High);
            Text = user.Name.Split(' ').First();
        }

        #endregion
    }
}

