using System;
using System.IO;
using System.Collections.Generic;
using Xwt;
using Xwt.Backends;

namespace ArtivityExplorer
{
    public class FileListView : ListView
    {
        #region Members

        private readonly ListStore _store;

        private readonly Dictionary<string, int> _rows = new Dictionary<string, int>();

        public readonly DataField<string> LastUsedField = new DataField<string>();

        public readonly DataField<TimeSpan> TotalTimeField = new DataField<TimeSpan>();

        public readonly DataField<string> FileNameField = new DataField<string>();

        public readonly DataField<string> FileUrlField = new DataField<string>();

        public FileListView PreviousView;

        public FileListView NextView;

        #endregion

        #region Constructors

        public FileListView()
        {
            // Initialize the list data.
            _store = new ListStore(LastUsedField, TotalTimeField, FileNameField, FileUrlField);

            DataSource = _store;

            CanGetFocus = false;

            InitializeComponent();
        }

        #endregion

        #region Methods

        protected virtual void InitializeComponent()
        {
            Margin = 0;

            // Do not show any headers.
            HeadersVisible = false;

            // Do not draw any borders.
            BorderVisible = false;

            // Initialize the list view.
            Columns.AddTextColumn(LastUsedField, "Last Used", Alignment.Start);
            Columns.AddTextColumn(FileNameField, "Name", Alignment.Start);
            Columns.AddTextColumn(TotalTimeField, "Editing Time", Alignment.Start);
        }

        public int Update(string fileUrl, string filePath, DateTime lastUsed, TimeSpan duration)
        {
            int row;

            if (_rows.ContainsKey(filePath))
            {
                row = _rows[filePath];

                TimeSpan d = _store.GetValue(row, TotalTimeField);

                _store.SetValue(row, TotalTimeField, d + duration);
            }
            else
            {
                row = _store.AddRow();

                _store.SetValues(row,
                    LastUsedField, "  " + lastUsed.ToString("t"),
                    TotalTimeField, duration,
                    FileNameField, Path.GetFileName(filePath),
                    FileUrlField, fileUrl);

                _rows[filePath] = row;
            }

            return row;
        }

        protected override void OnKeyReleased(KeyEventArgs e)
        {
            e.Handled = false;

            Parent.Parent.SetFocus();
        }

        protected override void OnRowActivated(ListViewRowEventArgs e)
        {
            string fileUrl = _store.GetValue(e.RowIndex, FileUrlField);

            RaiseFileSelected(fileUrl);
        }

        public string GetSelectedFile()
        {
            return _store.GetValue(SelectedRow, FileUrlField);
        }

        #endregion

        #region Events

        public FileSelectionEventHandler FileSelected { get; set; }

        private void RaiseFileSelected(string filename)
        {
            if (FileSelected == null) return;

            FileSelected(this, new FileSelectionEventArgs(filename));
        }

        #endregion
    }
}

