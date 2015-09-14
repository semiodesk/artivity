using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xwt;
using Xwt.Drawing;
using ArtivityExplorer.Parsers;

namespace ArtivityExplorer.Controls
{
    public class EditingWidget : Table
    {
        #region Members

        private readonly Label _sessionLabel = new Label("Sessions");

        private readonly Label _sessionCountLabel = new Label() { TextAlignment = Alignment.End, Text = "0" };

        private readonly Label _stepLabel = new Label("Steps");

        private readonly Label _stepCountLabel = new Label() { TextAlignment = Alignment.End, Text = "0" };

        private readonly Label _undoLabel = new Label("  Undos");

        private readonly Label _undoCountLabel = new Label() { TextAlignment = Alignment.End, Text = "0" };

        private readonly Label _redoLabel = new Label("  Redos");

        private readonly Label _redoCountLabel = new Label() { TextAlignment = Alignment.End, Text = "0" };

        private readonly Label _confidenceLabel = new Label("Confidence");

        private readonly Label _confidenceCountLabel = new Label() { TextAlignment = Alignment.End, Text = "0" };

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
            _stepCountLabel.TextColor = color;

            Add(_stepLabel, 1, 2, 1, 1, true);
            Add(_stepCountLabel, 2, 2);

            _undoLabel.TextColor = color;
            _undoCountLabel.TextColor = color;

            Add(_undoLabel, 1, 3, 1, 1, true);
            Add(_undoCountLabel, 2, 3);

            _redoLabel.TextColor = color;
            _redoCountLabel.TextColor = color;

            Add(_redoLabel, 1, 4, 1, 1, true);
            Add(_redoCountLabel, 2, 4);

            _confidenceLabel.TextColor = color;
            _confidenceCountLabel.TextColor = color;

            Add(_confidenceLabel, 1, 5, 1, 1, true);
            Add(_confidenceCountLabel, 2, 5);
        }

        #endregion
    }
}
