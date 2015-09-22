using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xwt;
using Xwt.Drawing;
using ArtivityExplorer.Parsers;
using Semiodesk.Trinity;
using Artivity.Model;

namespace ArtivityExplorer.Controls
{
    public class EditingWidget : Table
    {
        #region Members

        private readonly Label _sessionLabel = new Label("Sessions");

        private readonly Label _sessionCountLabel = new Label() { TextAlignment = Alignment.End, Text = "0" };

        private readonly Label _stepLabel = new Label("Steps");

        private readonly Label _updateCountLabel = new Label() { TextAlignment = Alignment.End, Text = "0" };

        private readonly Label _undoLabel = new Label("  Undos");

        private readonly Label _undoCountLabel = new Label() { TextAlignment = Alignment.End, Text = "0" };

        private readonly Label _redoLabel = new Label("  Redos");

        private readonly Label _redoCountLabel = new Label() { TextAlignment = Alignment.End, Text = "0" };

        private readonly Label _confidenceLabel = new Label("Confidence");

        private readonly Label _confidenceValueLabel = new Label() { TextAlignment = Alignment.End, Text = "0" };

        #endregion

        #region Constructors

        public EditingWidget()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void InitializeComponent()
        {
			Color color = Color.FromBytes(49, 55, 57);

			ImageView icon = new ImageView(BitmapImage.FromResource("edit"));

			Add(icon, 0, 0);

            Label title = new Label("Editing");
            title.Font = Font.WithWeight(FontWeight.Semibold);
            title.TextColor = color;

            Add(title, 1, 0);

            _sessionLabel.TextColor = color;
            _sessionCountLabel.TextColor = color;

            Add(_sessionLabel, 1, 1, 1, 1, true);
            Add(_sessionCountLabel, 2, 1);

            _stepLabel.TextColor = color;
            _updateCountLabel.TextColor = color;

            Add(_stepLabel, 1, 2, 1, 1, true);
            Add(_updateCountLabel, 2, 2);

            _undoLabel.TextColor = color;
            _undoCountLabel.TextColor = color;

            Add(_undoLabel, 1, 3, 1, 1, true);
            Add(_undoCountLabel, 2, 3);

            _redoLabel.TextColor = color;
            _redoCountLabel.TextColor = color;

            Add(_redoLabel, 1, 4, 1, 1, true);
            Add(_redoCountLabel, 2, 4);

            _confidenceLabel.TextColor = color;
            _confidenceValueLabel.TextColor = color;

            Add(_confidenceLabel, 1, 5, 1, 1, true);
            Add(_confidenceValueLabel, 2, 5);
        }

		public void Update(IModel model, string file)
		{
			ResourceQuery sessions = new ResourceQuery(art.Open);
			ResourceQuery updates = new ResourceQuery(art.Update);
			ResourceQuery undos = new ResourceQuery(art.Undo);
			ResourceQuery redos = new ResourceQuery(art.Redo);

			double sessionCount = model.ExecuteQuery(sessions).Count();
			double updateCount = model.ExecuteQuery(updates).Count();
			double undoCount = model.ExecuteQuery(undos).Count();
			double redoCount = model.ExecuteQuery(redos).Count();

			_sessionCountLabel.Text = sessionCount.ToString();
			_updateCountLabel.Text = updateCount.ToString();
			_undoCountLabel.Text = undoCount.ToString();
			_redoCountLabel.Text = redoCount.ToString();

			if (updateCount > 0)
			{
				_confidenceValueLabel.Text = Math.Round((updateCount - undoCount + redoCount) / updateCount, 2).ToString();
			}
			else
			{
				_confidenceValueLabel.Text = "0";
			}
		}

        #endregion
    }
}
