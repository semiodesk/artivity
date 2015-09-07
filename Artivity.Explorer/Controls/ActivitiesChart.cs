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

		private Dictionary<string, OxyColor> _actorColors = new Dictionary<string, OxyColor>();

		#endregion

        #region Constructors

        public ActivitiesChart()
        {
			BackgroundColor = new Color(0, 0, 0);
            MinHeight = 200;
            Margin = 0;

			_actorColors["application://inkscape.desktop"] = OxyColor.FromRgb(237, 44, 169);
			_actorColors["application://chromium-browser.desktop"] = OxyColor.FromRgb(44, 237, 169);
        }

        #endregion

        #region Methods

		public void Update(IEnumerable<Activity> activities)
		{
			List<LineSeries> series = new List<LineSeries>();
		
			double max = 1;

			series.Add(CreateSeries("application://inkscape.desktop", activities.Where(a => a.Actor != null && a.Actor.Uri.ToString() == "application://inkscape.desktop"), ref max));
			series.Add(CreateSeries("application://chromium-browser.desktop", activities.Where(a => a.Actor != null && a.Actor.Uri.ToString() == "application://chromium-browser.desktop"), ref max));

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
				Maximum = Math.Ceiling(max / 10) * 10,
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

			foreach (LineSeries s in series)
			{
				model.Series.Add(s);
			}

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
            edits.MarkerSize = 4;

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
			series.Color = _actorColors[actor];
			series.StrokeThickness = 2;
			series.MarkerSize = 4;

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

							int delta = Convert.ToInt32((currentTime - previousTime).TotalMinutes);

							for (int n = 1; n < delta; n++)
							{
								series.Points.Add(DateTimeAxis.CreateDataPoint(previousTime.AddMinutes(n), 0));
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
