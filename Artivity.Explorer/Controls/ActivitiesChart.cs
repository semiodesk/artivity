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

			List<LineSeries> series = new List<LineSeries>();
		
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

			foreach (KeyValuePair<Uri, List<Activity>> item in agents)
			{
				series.Add(CreateSeries(item.Key.AbsoluteUri, item.Value, ref max));
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

			y.Minimum = 0;
			y.AbsoluteMinimum = 0;
			y.Maximum = Math.Ceiling(max / 10) * 10;
			y.AbsoluteMaximum = max;
			y.PositionAtZeroCrossing = true;

			PlotModel model = new PlotModel();
			model.Title = "ACTIVITIES / MIN";
			model.TitleFontSize = 9;
			model.TitleFontWeight = 0.5;
			model.TitleColor = OxyColors.White;
			model.PlotAreaBorderColor = OxyColor.FromRgb(88, 88, 88);
			model.PlotAreaBorderThickness = new OxyThickness(0, 0, 0, 1);
			model.Axes.Add(x);
			model.Axes.Add(y);

			foreach (LineSeries s in series)
			{
				model.Series.Add(s);
			}

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

        public void Update()
        {
            Random random = new Random();

			DateTime now = DateTime.Now;

			// Prepare the editing session chart.
			PolygonAnnotation sessions = new PolygonAnnotation();
			sessions.Layer = AnnotationLayer.BelowAxes;
			sessions.Fill = OxyColor.FromArgb(125, 237, 44, 169);
			sessions.Points.Add(DateTimeAxis.CreateDataPoint(now.AddMinutes(10), 0));
			sessions.Points.Add(DateTimeAxis.CreateDataPoint(now.AddMinutes(30), 0));            
			sessions.Points.Add(DateTimeAxis.CreateDataPoint(now.AddMinutes(30), 100));
			sessions.Points.Add(DateTimeAxis.CreateDataPoint(now.AddMinutes(10), 100));

			// Prepare the editing an browsing line charts.
			LineSeries edits = new LineSeries();
			edits.Title = "Inkscape Vector Illustrator";
			edits.Color = OxyColor.FromRgb(237, 44, 169);
			edits.StrokeThickness = 2;

			for (int i = 0; i < 100; i++)
            {
				edits.Points.Add(DateTimeAxis.CreateDataPoint(now.AddMinutes(i), random.NextDouble() * 100));
            }

			LineSeries browsing = new LineSeries();
            browsing.Title = "Mozilla Firefox";
            browsing.Color = OxyColor.FromRgb(4, 197, 247);
            browsing.StrokeThickness = 2;

            for (int i = 0; i < 100; i++)
            {
				browsing.Points.Add(DateTimeAxis.CreateDataPoint(now.AddMinutes(i), random.NextDouble() * 100));
            }

			DateTimeAxis x = new DateTimeAxis()
			{
				Position = AxisPosition.Bottom,
				FontSize = 9,
				TextColor = OxyColors.LightGray,
				TicklineColor = OxyColors.LightGray,
				MajorTickSize = 1,
				MajorGridlineThickness = 0,
				MajorGridlineStyle = LineStyle.Dot,
				MajorGridlineColor = OxyColor.FromRgb(66, 66, 66),
				MinorTickSize = 60,
				IntervalType = DateTimeIntervalType.Days,
				MinorIntervalType = DateTimeIntervalType.Minutes,
				StringFormat = "HH:mm",
			};

			LinearAxis y = new LinearAxis()
			{
				Position = AxisPosition.Left,
				Minimum = 0,
				Maximum = 100,
				FontSize = 9,
				TextColor = OxyColors.LightGray,
				TicklineColor = OxyColors.LightGray,
                MajorTickSize = 10,
                MajorStep = 20,
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
			model.Series.Add(browsing);
            model.Series.Add(edits);
			model.Annotations.Add(sessions);

            model.LegendOrientation = LegendOrientation.Horizontal;
            model.LegendPlacement = LegendPlacement.Outside;
            model.LegendPosition = LegendPosition.BottomRight;
            model.LegendBackground = OxyColors.Transparent;
            model.LegendBorder = OxyColors.Transparent;
            model.LegendMargin = 0;
            model.LegendFontSize = 9;
            model.LegendTextColor = OxyColors.LightGray;

            Model = model;
        }

		private LineSeries CreateSeries(string actor, IEnumerable<Activity> activities, ref double max)
		{
			double i = 1;
			DateTime currentTime = DateTime.MinValue;
			DateTime previousTime = DateTime.MinValue;

			LineSeries series = new LineSeries();
			series.Title = actor;
			series.Color = _colors[_currentColor];
			series.StrokeThickness = 2;
			series.MarkerSize = 2;
			series.MarkerType = MarkerType.Circle;
			series.MarkerFill = OxyColor.FromRgb(49, 55, 57);;
			series.MarkerStroke = series.Color;
			series.MarkerStrokeThickness = 2;

			_currentColor++;

			if (activities.Any())
			{
				foreach (Activity activity in activities)
				{
					currentTime = activity.StartTime.RoundToMinute();

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

			return series;
		}

        #endregion
    }
}
