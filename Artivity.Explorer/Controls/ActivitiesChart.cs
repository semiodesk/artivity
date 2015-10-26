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
using System.Threading.Tasks;

namespace ArtivityExplorer.Controls
{
    public class ActivitiesChart : PlotView
    {
		#region Members

        private Dictionary<Uri, Agent> _agents = new Dictionary<Uri, Agent>();

        private Dictionary<Agent, OxyColor> _palette = new Dictionary<Agent, OxyColor>();

        private Dictionary<Agent, LineSeries> _series = new Dictionary<Agent, LineSeries>();

        private Dictionary<Agent, List<PolygonAnnotation>> _annotations = new Dictionary<Agent, List<PolygonAnnotation>>();

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
            IModel model = Models.GetAgents();

            ResourceQuery query = new ResourceQuery(prov.SoftwareAgent);

            foreach (SoftwareAgent agent in model.GetResources<SoftwareAgent>(query))
            {
                _agents[agent.Uri] = agent;

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
                MajorGridlineColor = OxyColor.FromRgb(88, 88, 88),
                MinorGridlineStyle = LineStyle.Dot,
                MinorGridlineThickness = 1,
                MinorGridlineColor = OxyColor.FromRgb(66, 66, 66),
                IsZoomEnabled = false,
                IsPanEnabled = false
            };

            Model = new PlotModel();
            Model.Title = "Influences / Min";
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

        public void LoadActivities(string fileUrl)
        {
            string queryString = @"
                PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                PREFIX nfo: <http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#>
                PREFIX prov: <http://www.w3.org/ns/prov#>

                SELECT ?agent ?startTime ?endTime WHERE
                {
                  ?activity prov:qualifiedAssociation ?association .
                  ?activity prov:startedAtTime ?startTime .
                  ?activity prov:endedAtTime ?endTime .
                  ?activity prov:used ?entity .

                  ?association prov:agent ?agent .

                  ?entity nfo:fileUrl """ + fileUrl + @""" .
                }
                ORDER BY DESC(?startTime)";

            IModel model = Models.GetAllActivities();

            SparqlQuery query = new SparqlQuery(queryString);
            ISparqlQueryResult result = model.ExecuteQuery(query, true);

            foreach (BindingSet binding in result.GetBindings())
            {
                Agent agent = new Agent(new Uri(binding["agent"].ToString()));
                DateTime startTime = ((DateTime)binding["startTime"]).RoundToMinute();
                DateTime endTime = ((DateTime)binding["endTime"]).RoundToMinute();

                OxyColor color = _palette[agent];

                // Since we're going backward in time, we see the close activities first.
                PolygonAnnotation annotation = new PolygonAnnotation();
                annotation.Layer = AnnotationLayer.BelowAxes;
                annotation.Fill = OxyColor.FromArgb(125, color.R, color.G, color.B);
                annotation.Points.Add(DateTimeAxis.CreateDataPoint(startTime, 0));
                annotation.Points.Add(DateTimeAxis.CreateDataPoint(startTime, 100));
                annotation.Points.Add(DateTimeAxis.CreateDataPoint(endTime, 100));
                annotation.Points.Add(DateTimeAxis.CreateDataPoint(endTime, 0));

                Model.Annotations.Add(annotation);
            }
        }

        public void LoadActivityInfluences(string fileUrl)
        {
            string queryString = @"
                PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                PREFIX nfo: <http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#>
                PREFIX prov: <http://www.w3.org/ns/prov#>

                SELECT ?agent ?influenceTime WHERE 
                {
                    ?activity prov:qualifiedAssociation ?association .

                    ?association prov:agent ?agent .

                    {
                        ?activity prov:used ?file ;
                                        prov:generated ?version .

                        ?file nfo:fileUrl """ + fileUrl + @""" .

                        ?version prov:qualifiedGeneration ?generation .

                        ?generation prov:atTime ?influenceTime .
                    }
                    UNION
                    {
                        ?editing prov:used ?file;
                                    prov:startedAtTime ?startTime ;
                                    prov:endedAtTime ?endTime .

                        ?file nfo:fileUrl """ + fileUrl + @""" .

                        ?activity prov:startedAtTime ?time ;
                            prov:qualifiedUsage ?usage .

                        ?usage prov:atTime ?influenceTime .

                        FILTER(?startTime <= ?time && ?time <= ?endTime) .
                    }
                }
                ORDER BY DESC(?influenceTime)";

            IModel model = Models.GetAllActivities();

            SparqlQuery query = new SparqlQuery(queryString);
            ISparqlQueryResult result = model.ExecuteQuery(query, true);

            CreateSeriesPoints(result);

            double max = Math.Ceiling(_maxY / 10) * 10;

            _y.Maximum = max;

            if (max == _maxY)
            {
                _y.Maximum += max <= 100 ? 5 : 10;
            }

            _y.MinorStep = max <= 100 ? 5 : 10;
        }
            
        private LineSeries CreateSeries(Agent agent)
        {
            OxyColor color = _palette[agent];

            LineSeries series = new LineSeries();
            series.Title = agent.Name;
            series.Color = color;
            series.StrokeThickness = 1.5;
            series.MarkerSize = 3;
            series.MarkerType = MarkerType.Circle;
            series.MarkerFill = color;
            series.MarkerStroke = OxyColor.FromRgb(49, 55, 57);
            series.MarkerStrokeThickness = 1;
            series.CanTrackerInterpolatePoints = true;
            series.Selectable = true;
            series.SelectionMode = OxyPlot.SelectionMode.Single;

            return series;
        }
            
        private void CreateSeriesPoints(ISparqlQueryResult result)
        {
            DateTime previousTime;

            foreach (BindingSet binding in result.GetBindings())
            {
                UriRef uri = new UriRef(binding["agent"].ToString());
                    
                Agent agent = _agents.ContainsKey(uri) ? _agents[uri] : new Agent(uri);

                // We initialize one series per agent.
                LineSeries series;

                if (!_series.ContainsKey(agent))
                {
                    series = CreateSeries(agent);

                    Model.Series.Add(series);

                    _series[agent] = series;

                    previousTime = DateTime.MinValue;
                }
                else
                {
                    series = _series[agent];

                    previousTime = DateTimeAxis.ToDateTime(series.Points.Last().X);
                }

                DateTime currentTime = (DateTime)binding["influenceTime"];

                currentTime = currentTime.RoundToMinute();

                if (previousTime != DateTime.MinValue)
                {
                    if (DateTime.Equals(currentTime, previousTime))
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
                        double delta = (previousTime - currentTime).TotalMinutes;

                        if (delta > 1)
                        {
                            DateTime t = previousTime.Subtract(TimeSpan.FromMinutes(1));

                            series.Points.Add(DateTimeAxis.CreateDataPoint(t, 0));
                        }

                        if (delta > 2)
                        {
                            DateTime t = currentTime.Add(TimeSpan.FromMinutes(1));

                            series.Points.Add(DateTimeAxis.CreateDataPoint(t, 0));
                        }

                        // ..and add the new point at the end.
                        series.Points.Add(DateTimeAxis.CreateDataPoint(currentTime, 1));
                    }
                }
                else
                {
                    // If there are no points in the series, we add this one.
                    DateTime zeroTime = currentTime.Add(TimeSpan.FromMinutes(1));

                    series.Points.Add(DateTimeAxis.CreateDataPoint(zeroTime, 0));
                    series.Points.Add(DateTimeAxis.CreateDataPoint(currentTime, 1));
                }

                previousTime = currentTime;
            }

            // Add a zero to the end of each line series.
            foreach (LineSeries series in _series.Values)
            {
                if (!series.Points.Any())
                    continue;

                previousTime = DateTimeAxis.ToDateTime(series.Points.Last().X);

                DateTime t = previousTime.Subtract(TimeSpan.FromMinutes(1));

                series.Points.Add(DateTimeAxis.CreateDataPoint(t, 0));
            }
        }

        #endregion
    }

    public delegate void AsyncLoadMethodCaller(IModel model, string file);
}
