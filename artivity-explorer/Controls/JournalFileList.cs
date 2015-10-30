using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Eto.Forms;
using System.Data;

namespace ArtivityExplorer
{
    public class JournalFileList : GridView
    {
        #region Members

        private readonly ObservableCollection<JournalFileListItem> _items = new ObservableCollection<JournalFileListItem>();

        private readonly Dictionary<string, JournalFileListItem> _fileItems = new Dictionary<string, JournalFileListItem>();
       
        private readonly Scrollable _container;

        #endregion

        #region Constructors

        public JournalFileList(Scrollable container)
        {
            _container = container;

            DataStore = _items;

            Columns.Add(new GridColumn() { DataCell = new TextBoxCell("FormattedLastEditingDate"), HeaderText = "Last Used", Width = 80 });
            Columns.Add(new GridColumn() { DataCell = new TextBoxCell("Path"), HeaderText = "File Name", Width = 500 });
            Columns.Add(new GridColumn() { DataCell = new TextBoxCell("FormattedTotalEditingTime"), HeaderText = "Editing Time", Width = 80 });

            // Do not show any headers.
            ShowHeader = false;
        }

        #endregion

        #region Methods

        public void Add(JournalFileListItem item)
        {
            if (_fileItems.ContainsKey(item.Path))
            {
                item = _fileItems[item.Path];
                item.TotalEditingTime += item.TotalEditingTime;
            }
            else
            {
                _items.Add(item);
                _fileItems[item.Path] = item;
            }
        }

        public JournalFileListItem GetSelectedItem()
        {
            return SelectedItem as JournalFileListItem;
        }

        protected override void OnGotFocus(EventArgs e)
        {
            _container.Focus();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _container.Focus();

            e.Handled = true;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            _container.Focus();

            e.Handled = true;
        }

        #endregion
    }
}

