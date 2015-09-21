using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Annotations;
using OxyPlot.Series;
using OxyPlot.Xwt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Semiodesk.Trinity;
using Xwt;
using Xwt.Drawing;
using Artivity.Model.ObjectModel;

namespace ArtivityExplorer.Controls
{
    public class ActivitiesChart : PlotView
    {
		#region Members

		private List<OxyColor> _colors = new List<OxyColor>();

		private int _currentColor = 0;

		#endregion

        #region Constructors

        public ActivitiesChart()
        {
			BackgroundColor = Color.FromBytes(49, 55, 57);
            MinHeight = 200;
            Margin = 0;

			_colors.Add(OxyColor.FromRgb(237, 20, 91)); // Crimson
			_colors.Add(OxyColor.FromRgb(80, 186, 226)); // Malibu
			_colors.Add(OxyColor.FromRgb(136, 199, 68)); // Christi
			_colors.Add(OxyColor.FromRgb(254, 194, 18)); // Moon yellow
			_colors.Add(OxyColor.FromRgb(248, 97, 68)); // Tomato
			_colors.Add(OxyColor.FromRgb(128, 57, 122)); // Seance
        }

        #endregion

        #region Methods

		public void Update(IEnumerable<Activity> activities)
		{
			_currentColor = 0;
		
			double max = 1;

			Dictionary<Uri, List<Activity>> agents = new Dictionary<Uri, List<Activity>>();

			foreach (Activity activity in activities)
			{
				Association association = activity.Associations.FirstOrDefault();

				if (association == null || association.Agent == null) continue;

				Agent agent = association.Agent;

				if (!agents.ContainsKey(agent.Uri))
				{
					agents[agent.Uri] = new List<Activity>();
				}

				agents[agent.Uri].Add(activity);
			}

			DateTimeAxis x = new DateTimeAxis()
			{
				Position = AxisPosition.Bottom,
				FontSize = 9,
				TextColor = OxyColors.LightGray,
				TicklineColor = OxyColors.LightGray,
				IntervalType = DateTimeIntervalType.Days,
				MajorTickSize = 1,
				MajorGridlineThickness = 0,
				MajorGridlineStyle = LineStyle.Dot,
				MajorGridlineColor = OxyColor.FromRgb(66, 66, 66),
				MinorTickSize = 60,
				MinorIntervalType = DateTimeIntervalType.Minutes,
				StringFormat = "HH:mm",
			};

			LinearAxis y = new LinearAxis()
			{
				Position = AxisPosition.Left,
				FontSize = 9,
				TextColor = OxyColors.LightGray,
				TicklineColor = OxyColors.LightGray,
				MajorTickSize = 10,
				MajorStep = 5,
				MajorGridlineStyle = LineStyle.Dot,
				MajorGridlineThickness = 1.0,
				MajorGridlineColor = OxyColor.FromRgb(66, 66, 66)
			};

			PlotModel model = new PlotModel();
			model.Title = "ACTIVITIES / MIN";
			model.TitleFontSize = 9;
			model.TitleFontWeight = 0.5;
			model.TitleColor = OxyColors.White;
			model.PlotAreaBorderColor = OxyColor.FromRgb(88, 88, 88);
			model.PlotAreaBorderThickness = new OxyThickness(0, 0, 0, 1);
			model.Axes.Add(x);
			model.Axes.Add(y);

			foreach (KeyValuePair<Uri, List<Activity>> item in agents)
			{
				CreateSeries(model, item.Key.AbsoluteUri, item.Value, ref max);
			}

			y.Minimum = 0;
			y.AbsoluteMinimum = 0;
			y.Maximum = Math.Ceiling(max / 10) * 10;
			y.AbsoluteMaximum = max;
			y.PositionAtZeroCrossing = true;

			model.LegendOrientation = LegendOrientation.Horizontal;
			model.LegendPlacement = LegendPlacement.Outside;
			model.LegendPosition = LegendPosition.BottomCenter;
			model.LegendBackground = OxyColors.Transparent;
			model.LegendBorder = OxyColors.Transparent;
			model.LegendMargin = 0;
			model.LegendFontSize = 9;
			model.LegendTextColor = OxyColors.LightGray;

			Model = model;
		}

		private void CreateSeries(PlotModel model, string actor, IEnumerable<Activity> activities, ref double max)
		{
			double i = 1;
			DateTime currentTime = DateTime.MinValue;
			DateTime previousTime = DateTime.MinValue;

			OxyColor color = _colors[_currentColor];

			_currentColor++;

			LineSeries series = new LineSeries();
			series.Title = actor;
			series.Color = color;
			series.StrokeThickness = 2;
			series.MarkerSize = 2;
			series.MarkerType = MarkerType.Circle;
			series.MarkerFill = OxyColor.FromRgb(49, 55, 57);;
			series.MarkerStroke = series.Color;
			series.MarkerStrokeThickness = 2;

			PolygonAnnotation annotations = new PolygonAnnotation();
			annotations.Layer = AnnotationLayer.BelowAxes;
			annotations.Fill = OxyColor.FromArgb(125, color.R, color.G, color.B);

			if (activities.Any())
			{
				Open open = null;
				Close close = null;

				foreach (Activity activity in activities)
				{
					currentTime = activity.StartTime.RoundToMinute();

					if (activity is Open)
					{
						open = activity as Open;
					}

					if (activity is Close)
					{
						close = activity as Close;
					}

					if (open != null && close != null)
					{
						DateTime openTime = open.StartTime.RoundToMinute();
						DateTime closeTime = close.StartTime.RoundToMinute();

						if (openTime == closeTime)
						{
							closeTime = openTime.AddMinutes(1);
						}

						annotations.Points.Add(DateTimeAxis.CreateDataPoint(openTime, 0));
						annotations.Points.Add(DateTimeAxis.CreateDataPoint(openTime, 500));           
						annotations.Points.Add(DateTimeAxis.CreateDataPoint(closeTime, 500));
						annotations.Points.Add(DateTimeAxis.CreateDataPoint(closeTime, 0)); 

						open = null;
						close = null;
					}

					if (previousTime != DateTime.MinValue)
					{
						if (currentTime != previousTime)
						{
							series.Points.Add(DateTimeAxis.CreateDataPoint(previousTime, i));

							double delta = (previousTime - currentTime).TotalMinutes;

							for (double m = 1.0; m < delta; m++)
							{
								DateTime t = previousTime.Subtract(TimeSpan.FromMinutes(m));

								series.Points.Add(DateTimeAxis.CreateDataPoint(t, 0));
							}

							i = 1;
						}
						else
						{
							i += 1;
							max = i > max ? i : max;
						}
					}

					previousTime = currentTime;
				}

				series.Points.Add(DateTimeAxis.CreateDataPoint(currentTime, i));
			}

			if (annotations.Points.Any())
			{
				model.Annotations.Add(annotations);
			}

			if (series.Points.Any())
			{
				model.Series.Add(series);
			}
		}

        #endregion
    }
}
