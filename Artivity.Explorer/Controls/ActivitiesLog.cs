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

		private IModel _model;

		public TreeStore Store { get; private set; }

		public IDataField<Activity> ActivityField { get; private set; }
		public IDataField<DateTime> TimeField { get; private set; }
		public IDataField<string> TypeField { get; private set; }
		public IDataField<string> TargetField { get; private set; }
		public IDataField<string> ModificationTypeField { get; private set; }
		public IDataField<string> ModificationFromField { get; private set; }
		public IDataField<string> ModificationToField { get; private set; }
		public IDataField<string> ZoomField { get; private set; }

		#endregion

        #region Constructors

        public ActivitiesLog(IModel model)
        {
            InitializeComponent();

			_model = model;
        }

        #endregion

        #region Methods

        private void InitializeComponent()
        {
            Margin = 0;
            HeadersVisible = true;
            SelectionMode = SelectionMode.Multiple;

			ActivityField = new DataField<Activity>();
            TimeField = new DataField<DateTime>();
            TypeField = new DataField<string>();
			TargetField = new DataField<string>();
            ModificationTypeField = new DataField<string>();
			ModificationFromField = new DataField<string>();
			ModificationToField = new DataField<string>();
			ZoomField = new DataField<string>();

            Columns.Add(new ListViewColumn("Time", new TextCellView(TimeField)) { Alignment = Alignment.Start });
            Columns.Add(new ListViewColumn("Event", new TextCellView(TypeField)));
			Columns.Add(new ListViewColumn("Target", new TextCellView(TargetField)) { CanResize = true });
            Columns.Add(new ListViewColumn("Modification", new TextCellView(ModificationTypeField)));
			Columns.Add(new ListViewColumn("From", new TextCellView(ModificationFromField)));
			Columns.Add(new ListViewColumn("To", new TextCellView(ModificationToField)));
			Columns.Add(new ListViewColumn("Zoom", new TextCellView(ZoomField)));

			Store = new TreeStore(ActivityField, TimeField, TypeField, TargetField, ModificationTypeField, ModificationFromField, ModificationToField, ZoomField);

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
