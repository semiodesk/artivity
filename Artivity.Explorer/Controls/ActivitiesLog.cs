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

        public void Reset()
        {
            Store.Clear();
        }

        public void Add(Activity activity)
        {
            TreeNavigator node = Store.AddNode();

            // Set the data context of the current row.
            node.SetValue(ActivityField, activity);

            // Set the formatted date time.
            node.SetValue(TimeField, activity.StartTime.ToString("HH:mm:ss")); 

            if (activity.GetTypes().Any())
            {
                node.SetValue(TypeField, "<b>" + ToDisplayString(activity.GetTypes().First().Uri) + "</b>");
            }

            // Set activity action details.
            FileDataObject f = activity.GeneratedEntities.OfType<FileDataObject>().FirstOrDefault();

            if (f != null)
            {
                string action = "";

                if (f.Generation.Location is XmlAttribute)
                {
                    XmlAttribute attribute = f.Generation.Location as XmlAttribute;

                    action += attribute.LocalName + ": ";
                }

                if (f.Generation.Value != null)
                {
                    action += f.Generation.Value;
                }

                node.SetValue(ActionField, action);

                if (f.Generation.Viewbox != null)
                {
                    // Set the formatted zoom value.
                    node.SetValue(ZoomField, Math.Round(f.Generation.Viewbox.ZoomFactor * 100, 0) + "%");
                }
            }
            else
            {
                WebDataObject w = activity.UsedEntities.OfType<WebDataObject>().FirstOrDefault();

                if (w != null)
                {
                    node.SetValue(ActionField, w.Uri.Host);
                }
            }
        }

        private string ToDisplayString(Uri uri)
        {
            string result = uri.AbsoluteUri;

            if (result.Contains('#'))
            {
                result = result.Substring(result.LastIndexOf('#') + 1);
            }
            else if(uri.AbsoluteUri.Contains('/'))
            {
                result = result.Substring(result.LastIndexOf('/') + 1);
            }

            return result.TrimEnd('>');
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
