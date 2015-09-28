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
using Artivity.Model;

namespace ArtivityExplorer.Controls
{
    public class ActivitiesChart : PlotView
    {
		#region Members

        private Dictionary<Agent, OxyColor> _palette = new Dictionary<Agent, OxyColor>();

        private Dictionary<Agent, LineSeries> _series = new Dictionary<Agent, LineSeries>();

        private Dictionary<Agent, List<PolygonAnnotation>> _annotations = new Dictionary<Agent, List<PolygonAnnotation>>();

        private Close _lastClose;

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

            InitializeAgents();

            Reset();
        }

        #endregion

        #region Methods

        private void InitializeAgents()
        {
            IStore store = StoreFactory.CreateStoreFromConfiguration("virt0");

            IModel model;

            if (store.ContainsModel(Models.Agents))
            {
                model = store.GetModel(Models.Agents);
            }
            else
            {
                model = store.CreateModel(Models.Agents);
            }

            ResourceQuery query = new ResourceQuery(prov.SoftwareAgent);

            foreach (SoftwareAgent agent in model.GetResources<SoftwareAgent>(query))
            {
                System.Drawing.Color c = System.Drawing.ColorTranslator.FromHtml(agent.ColourCode);

                _palette[agent] = OxyColor.FromArgb(c.A, c.R, c.G, c.B);
            }
        }

        public void Reset()
        {
            Clear();

            _series.Clear();
			_annotations.Clear();
			_previous.Clear();

			_maxY = 1;

            _x = new DateTimeAxis()
            {
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

            // We initialize one series per agent.
            LineSeries series;

			if (!_series.ContainsKey(agent))
            {
				series = CreateSeries(agent);

				_series[agent] = series;
            }
            else
            {
                series = _series[agent];
            }

            if (_previous.ContainsKey(agent))
            {
                // If there are already points in the series, we add the current one..
                DateTime previous = _previous[agent];

                if (DateTime.Equals(current, previous))
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
            }
            else
            {
                // If there are no points in the series, we add this one.
                DateTime zero = current.Add(TimeSpan.FromMinutes(1));

                series.Points.Add(DateTimeAxis.CreateDataPoint(zero, 0));
                series.Points.Add(DateTimeAxis.CreateDataPoint(current, 1));
            }

            if (_lastClose == null && activity is Close)
            {
                // We move both, open and close a minute from each other so that the region
                // is always visible in case both events happen at the same minute.
                DateTime close = current.Add(TimeSpan.FromMinutes(1));

                // If there are no annotations for this client yet, initialize them now.
                if (!_annotations.ContainsKey(agent))
                {
                    _annotations.Add(agent, new List<PolygonAnnotation>());
                }

                // Add the background shade to the open and close sessions.
                OxyColor color = _palette[agent];

                // Since we're going backward in time, we see the close activities first.
                PolygonAnnotation annotation = new PolygonAnnotation();
                annotation.Layer = AnnotationLayer.BelowAxes;
                annotation.Fill = OxyColor.FromArgb(125, color.R, color.G, color.B);
                annotation.Points.Add(DateTimeAxis.CreateDataPoint(close, 0));
                annotation.Points.Add(DateTimeAxis.CreateDataPoint(close, 100));

                _annotations[agent].Add(annotation);

                _lastClose = activity as Close;
            }
			else if (_lastClose != null && activity is Open)
			{
                DateTime open = current.Subtract(TimeSpan.FromMinutes(1));

                if (_annotations.ContainsKey(agent))
                {
                    PolygonAnnotation annotation = _annotations[agent].Last();
                    annotation.Points.Add(DateTimeAxis.CreateDataPoint(open, 100));
                    annotation.Points.Add(DateTimeAxis.CreateDataPoint(open, 0));
                }

                _lastClose = null;
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

            foreach (PolygonAnnotation annoation in _annotations.Values.SelectMany(a => a))
            {
                Model.Annotations.Add(annoation);
            }
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
            series.Title = agent.Name;

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
