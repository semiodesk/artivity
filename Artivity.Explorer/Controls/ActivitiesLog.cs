using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xwt;
using Semiodesk.Trinity;
using Artivity.Model.ObjectModel;
using Artivity.Model;

namespace ArtivityExplorer.Controls
{
    public class ActivitiesLog : TreeView
    {
		#region Members
		
		public TreeStore Store { get; private set; }

		public IDataField<Activity> ActivityField { get; private set; }
		public IDataField<string> TimeField { get; private set; }
		public IDataField<string> TypeField { get; private set; }
		public IDataField<string> ActionField { get; private set; }
		public IDataField<string> ZoomField { get; private set; }

		#endregion

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

			ActivityField = new DataField<Activity>();
            TimeField = new DataField<string>();
            TypeField = new DataField<string>();
			ActionField = new DataField<string>();
			ZoomField = new DataField<string>();

			TextCellView timeView = new TextCellView();
			timeView.MarkupField = TimeField;

			TextCellView typeView = new TextCellView();
			typeView.MarkupField = TypeField;

			TextCellView actionView = new TextCellView();
			actionView.MarkupField = ActionField;

			TextCellView zoomView = new TextCellView();
			zoomView.MarkupField = ZoomField;

            Columns.Add(new ListViewColumn("Time", timeView));
            Columns.Add(new ListViewColumn("Event", typeView));
			Columns.Add(new ListViewColumn("Target", actionView));
			Columns.Add(new ListViewColumn("Zoom", zoomView));

			Store = new TreeStore(ActivityField, TimeField, TypeField, ActionField, ZoomField);

            DataSource = Store;

			foreach (ListViewColumn c in Columns)
			{
				c.Alignment = Alignment.Start;
				c.CanResize = true;
			}
        }

		protected override void OnKeyReleased(KeyEventArgs args)
		{
			base.OnKeyReleased(args);

			if (args.Key == Key.Delete)
			{
				IStore store = StoreFactory.CreateStoreFromConfiguration("virt0");

				foreach(TreePosition pos in SelectedRows)
				{
					TreeNavigator node = Store.GetNavigatorAt(pos);

					Activity activity = node.GetValue(ActivityField);

					IModel model;

					if (activity.UsedEntities.OfType<WebDataObject>().Any())
					{
						model = store.GetModel(Models.WebActivities);
					}
					else
					{
						model = store.GetModel(Models.Activities);
					}

					model.DeleteResource(activity);

					node.Remove();
				}
			}
		}

        #endregion
    }
}
