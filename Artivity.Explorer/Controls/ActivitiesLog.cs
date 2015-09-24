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

			TextCellView timeView = new TextCellView();
			timeView.MarkupField = TimeField;

			ListViewColumn timeColumn = new ListViewColumn ("Time", timeView);
			timeColumn.Alignment = Alignment.End;
            timeColumn.CanResize = true;

			TypeField = new DataField<string>();

			TextCellView typeView = new TextCellView();
			typeView.MarkupField = TypeField;

			ListViewColumn typeColumn = new ListViewColumn ("Event", typeView);
			typeColumn.Alignment = Alignment.Start;
            typeColumn.CanResize = true;

			ActionField = new DataField<string>();

			TextCellView actionView = new TextCellView();
			actionView.MarkupField = ActionField;

			ListViewColumn actionColumn = new ListViewColumn ("Details", actionView);
            actionColumn.Alignment = Alignment.Start;
            actionColumn.CanResize = true;

			ZoomField = new DataField<string>();

			TextCellView zoomView = new TextCellView();
			zoomView.MarkupField = ZoomField;

			ListViewColumn zoomColumn = new ListViewColumn ("Zoom", zoomView);
			zoomColumn.Alignment = Alignment.End;
            zoomColumn.CanResize = true;

            Columns.Add(typeColumn);
			Columns.Add(actionColumn);
			Columns.Add(zoomColumn);
			Columns.Add(timeColumn);

			Store = new TreeStore(ActivityField, TimeField, TypeField, ActionField, ZoomField);

            DataSource = Store;
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
