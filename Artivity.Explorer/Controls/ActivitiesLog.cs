using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xwt;

namespace ArtivityExplorer.Controls
{
    public class ActivitiesLog : TreeView
    {
        #region Constructors

        public ActivitiesLog()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void InitializeComponent()
        {
            Margin = 0;
            HeadersVisible = true;
            SelectionMode = SelectionMode.Multiple;

            IDataField<DateTime> timeField = new DataField<DateTime>();
            IDataField<string> typeField = new DataField<string>();
            IDataField<string> dataField = new DataField<string>();

            Columns.Add(new ListViewColumn("Time", new TextCellView(timeField)) { Alignment = Alignment.Start });
            Columns.Add(new ListViewColumn("Event", new TextCellView(typeField)));
            Columns.Add(new ListViewColumn("Data", new TextCellView(dataField)));

            TreeStore store = new TreeStore(timeField, typeField, dataField);

            TreeNavigator navigator = store.AddNode();
            navigator.SetValue(timeField, DateTime.UtcNow);
            navigator.SetValue(typeField, "art:EditEvent");
            navigator.SetValue(dataField, "<artsvg:changeAttribute key='cx' old='445.35712' new='445'/>");

            DataSource = store;
        }

        #endregion
    }
}
