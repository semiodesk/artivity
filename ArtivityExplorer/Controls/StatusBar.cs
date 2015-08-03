using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xwt;

namespace ArtivityExplorer.Controls
{
    public class StatusBar : HBox
    {
        #region Members

        private readonly Label _nameLabel = new Label();

        private readonly Label _createdLabel = new Label() { TextAlignment = Alignment.Center };

        private readonly Label _modifiedLabel = new Label() { TextAlignment = Alignment.Center };

        private readonly Label _accessedLabel = new Label() { TextAlignment = Alignment.Center };

        #endregion

        #region Constructors

        public StatusBar()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void InitializeComponent()
        {
            Margin = new WidgetSpacing(7, 5, 7, 5);
            Spacing = 30;

            PackStart(_nameLabel, true);
            PackStart(_createdLabel);
            PackStart(_modifiedLabel);
            PackStart(_accessedLabel);
        }

        public void Update(string filename)
        {
            if (!File.Exists(filename)) return;

            FileInfo info = new FileInfo(filename);

            _nameLabel.Text = info.FullName;
            _createdLabel.Text = info.CreationTime.ToString();
            _modifiedLabel.Text = info.LastWriteTime.ToString();
            _accessedLabel.Text = info.LastAccessTime.ToString();
        }

        #endregion
    }
}
