using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Xwt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xwt;
using Xwt.Drawing;
using OxyPlot.Annotations;

namespace ArtivityExplorer.Controls
{
    public class ActivitiesChart : PlotView
    {
        #region Constructors

        public ActivitiesChart()
        {
			BackgroundColor = new Color(0, 0, 0);
            MinHeight = 200;
            Margin = 0;

            Update();
        }

        #endregion

        #region Methods

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
				MajorGridlineThickness = 1,
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
				MajorGridlineStyle = LineStyle.Dot,
				MajorGridlineThickness = 1.0,
				MajorGridlineColor = OxyColor.FromRgb(66, 66, 66)
			};

            PlotModel model = new PlotModel();
            model.Title = "ACTIVITIES / MIN";
            model.TitleFontSize = 9;
            model.TitleFontWeight = 0.5;
            model.TitleColor = OxyColors.LightGray;
            model.PlotAreaBorderColor = OxyColor.FromRgb(88, 88, 88);
            model.PlotAreaBorderThickness = new OxyThickness(1, 0, 0, 1);
            model.Axes.Add(x);
            model.Axes.Add(y);
			model.Series.Add(browsing);
            model.Series.Add(edits);
			model.Annotations.Add(sessions);

            model.LegendOrientation = LegendOrientation.Horizontal;
            model.LegendPlacement = LegendPlacement.Outside;
            model.LegendPosition = LegendPosition.TopRight;
            model.LegendBackground = OxyColors.Transparent;
            model.LegendBorder = OxyColors.Transparent;
            model.LegendFontSize = 9;
            model.LegendTextColor = OxyColors.White;

            Model = model;
        }

        #endregion
    }
}
