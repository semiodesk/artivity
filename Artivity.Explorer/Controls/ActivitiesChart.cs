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

        private Dictionary<Agent, OxyColor> _palette = new Dictionary<Agent, OxyColor>()
        {
            { new Agent(new Uri("application://inkscape.desktop/")), OxyColor.FromRgb(237, 20, 91) },
            { new Agent(new Uri("application://krita.desktop/")), OxyColor.FromRgb(136, 199, 68) },
            { new Agent(new Uri("application://chromium-browser.desktop/")), OxyColor.FromRgb(17, 158, 218) },
            { new Agent(new Uri("application://firefox-browser.desktop/")), OxyColor.FromRgb(17, 158, 218) },
        };

        private Dictionary<Agent, LineSeries> _series = new Dictionary<Agent, LineSeries>();

		private Dictionary<Agent, Annotation> _annotations = new Dictionary<Agent, Annotation>();

		private Dictionary<Agent, DateTime> _previous = new Dictionary<Agent, DateTime>();

        private Axis _x;

        private Axis _y;

        private double _maxY;

		#endregion

        #region Constructors

        public ActivitiesChart()
        {
			BackgroundColor = Color.FromBytes(49, 55, 57);
            MinHeight = 150;
            Margin = 0;

            Reset();
        }

        #endregion

        #region Methods

        public void Reset()
        {
            Clear();

            _series.Clear();
			_annotations.Clear();
			_previous.Clear();

			_maxY = 1;

            _x = new DateTimeAxis()
            {
                Minimum = 0,
                Position = AxisPosition.Bottom,
                FontSize = 9,
                TextColor = OxyColors.LightGray,
                TicklineColor = OxyColors.LightGray,
                IntervalType = DateTimeIntervalType.Days,
                MajorGridlineColor = OxyColor.FromRgb(66, 66, 66),
                MinorTickSize = 60,
                MinorIntervalType = DateTimeIntervalType.Minutes,
                StringFormat = "HH:mm",
                IsZoomEnabled = true,
                IsPanEnabled = true
            };

            _y = new LinearAxis()
            {
                Position = AxisPosition.Left,
                FontSize = 9,
                TextColor = OxyColors.LightGray,
                TicklineColor = OxyColors.LightGray,
				Maximum = 30,
                MajorTickSize = 10,
                MajorStep = 10,
                MajorGridlineStyle = LineStyle.Dot,
                MajorGridlineThickness = 1,
                MajorGridlineColor = OxyColor.FromRgb(66, 66, 66),
                IsZoomEnabled = false,
                IsPanEnabled = false
            };

            Model = new PlotModel();
            Model.Title = "ACTIVITIES / MIN";
            Model.TitleFontSize = 9;
            Model.TitleFontWeight = 0.5;
            Model.TitleColor = OxyColors.White;
            Model.PlotAreaBorderColor = OxyColor.FromRgb(88, 88, 88);
            Model.PlotAreaBorderThickness = new OxyThickness(0, 0, 0, 1);
            Model.LegendOrientation = LegendOrientation.Horizontal;
            Model.LegendPlacement = LegendPlacement.Outside;
            Model.LegendPosition = LegendPosition.BottomCenter;
            Model.LegendBackground = OxyColors.Transparent;
            Model.LegendBorder = OxyColors.Transparent;
            Model.LegendMargin = 0;
            Model.LegendFontSize = 9;
            Model.LegendTextColor = OxyColors.LightGray;
            Model.Axes.Add(_x);
            Model.Axes.Add(_y);
        }

        public void Add(Activity activity)
        {
            Agent agent = activity.Associations.Select(a => a.Agent).FirstOrDefault();

			if (agent == null) return;
            
			DateTime current = activity.StartTime.RoundToMinute();

			if (!_series.ContainsKey(agent))
            {
				LineSeries series = CreateSeries(agent);

				series.Points.Add(DateTimeAxis.CreateDataPoint(current.Add(TimeSpan.FromMinutes(1)), 0));
				series.Points.Add(DateTimeAxis.CreateDataPoint(current, 1));

				_series[agent] = series;
            }
			else
			{
	            LineSeries series = _series[agent];

				DateTime previous = _previous[agent];

				if(DateTime.Equals(current, previous))
	            {
					// We increment the current data point's value..
					DataPoint p = series.Points.Last();

					series.Points.Remove(p);

					p.Y++;

					series.Points.Add(p);

	                _maxY = Math.Max(_maxY, p.Y);
	            }
				else
	            {
	                // We fill up the gapping minutes between the current time value and the previous one..
					double delta = (previous - current).TotalMinutes;

					for (double m = 1; m < delta; m++)
	                {
						DateTime t = previous.Subtract(TimeSpan.FromMinutes(m));

	                    series.Points.Add(DateTimeAxis.CreateDataPoint(t, 0));
	                }

	                // ..and add the new point at the end.
					series.Points.Add(DateTimeAxis.CreateDataPoint(current, 1));
	            }

				if (activity is Open)
				{
					DateTime zero = current.Subtract(TimeSpan.FromMinutes(1));
	
					series.Points.Add(DateTimeAxis.CreateDataPoint(zero, 0));

					// Add the background shade to the open and close sessions.
//					OxyColor color = _palette[agent];
//
//					PolygonAnnotation annotation = new PolygonAnnotation();
//					annotation.Layer = AnnotationLayer.BelowAxes;
//					annotation.Fill = OxyColor.FromArgb(125, color.R, color.G, color.B);
//
//					_annotations[agent] = annotation;
				}
				else if (activity is Close)
				{
					// TODO
				}
			}

			_previous[agent] = current;
        }

        public void Draw()
        {
            foreach (LineSeries series in _series.Values)
            {
                Model.Series.Add(series);
            }

			_y.Maximum = Math.Ceiling(_maxY / 10) * 10;
        }

        private LineSeries CreateSeries(Agent agent)
        {
            LineSeries series = new LineSeries();
            series.Color = _palette[agent];
            series.StrokeThickness = 2;
            series.MarkerSize = 2;
            series.MarkerType = MarkerType.Circle;
            series.MarkerFill = OxyColor.FromRgb(49, 55, 57);;
            series.MarkerStroke = series.Color;
            series.MarkerStrokeThickness = 2;
            series.Title = agent.Uri.AbsoluteUri;

            return series;
        }

		private void CreateSeries(PlotModel model, Agent agent, IEnumerable<Activity> activities, ref double max)
		{
			double i = 1;
			DateTime currentTime = DateTime.MinValue;
			DateTime previousTime = DateTime.MinValue;

            OxyColor color = _palette[agent];

			LineSeries series = new LineSeries();
			series.Title = agent.Uri.AbsoluteUri;
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
