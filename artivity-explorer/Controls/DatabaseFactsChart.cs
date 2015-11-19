// LICENSE:
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// AUTHORS:
//
//  Sebastian Faubel <sebastian@semiodesk.com>
//
// Copyright (c) Semiodesk GmbH 2015

using System;
using System.Collections.Generic;
using Semiodesk.Trinity;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using Eto.OxyPlot;
using Eto.Drawing;
using Artivity.DataModel;

namespace Artivity.Explorer
{
    public class DatabaseFactsChart : Plot
    {
        #region Members

        private Axis _x;

        private Axis _y;

        public double AverageDelta = 0;

        #endregion

        #region Constructors

        public DatabaseFactsChart()
        {
            BackgroundColor = Colors.Transparent;

            Reset();
        }

        #endregion

        #region Methods

        public void Reset()
        {
            SuspendLayout();

            Font font = SystemFonts.Label(12);

            OxyColor axisTextColor = Palette.TextColor.WithA(0.85f).ToOxyColor();
            OxyColor axisLineColor = Palette.DarkColor.WithA(0.25f).ToOxyColor();

            _x = new DateTimeAxis()
            {
                Position = AxisPosition.Bottom,
                Font = font.FamilyName,
                FontSize = font.Size,
                FontWeight = 800,
                TextColor = axisTextColor,
                TicklineColor = axisLineColor,
                IntervalType = DateTimeIntervalType.Days,
                MajorTickSize = 1,
                MajorGridlineColor = axisLineColor,
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineThickness = 1,
                StringFormat = "dd.MM",
                IsZoomEnabled = true,
                IsPanEnabled = true,
            };

            _y = new LinearAxis()
            {
                Position = AxisPosition.Left,
                Font = font.FamilyName,
                FontSize = font.Size,
                FontWeight = 800,
                TextColor = axisTextColor,
                TicklineColor = axisLineColor,
                Maximum = 1000,
                Minimum = 0,
                MajorStep = 200,
                MajorTickSize = 1,
                MajorGridlineStyle = LineStyle.Dash,
                MajorGridlineThickness = 1,
                MajorGridlineColor = axisLineColor,
                IsZoomEnabled = false,
                IsPanEnabled = false,
            };

            Model = new PlotModel();
            Model.PlotAreaBorderColor = Palette.DarkColor.WithA(0.75f).ToOxyColor();
            Model.PlotAreaBorderThickness = new OxyThickness(0, 0, 0, 1);
            Model.LegendOrientation = LegendOrientation.Horizontal;
            Model.LegendPlacement = LegendPlacement.Outside;
            Model.LegendPosition = LegendPosition.BottomRight;
            Model.LegendBackground = OxyColors.Transparent;
            Model.LegendBorder = OxyColors.Transparent;
            Model.LegendMargin = 0;
            Model.LegendFontSize = 10;
            Model.LegendTextColor = Palette.TextColor.ToOxyColor();
            Model.Axes.Add(_x);
            Model.Axes.Add(_y);

            ResumeLayout();
        }
            
        public void Update()
        {
            SuspendLayout();

            Model.Series.Clear();

            string queryString = @"
                PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                PREFIX nfo: <http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#>
                PREFIX art: <http://semiodesk.com/artivity/1.0/>

                SELECT ?time ?facts ?size WHERE
                {
                  ?database art:hadState ?state .

                  ?state art:atTime ?time .
                  ?state art:factsCount ?facts .
                  ?state nfo:fileSize ?size .
                }
                ORDER BY ASC(?time)";


            IModel model = Models.GetMonitoring();

            SparqlQuery query = new SparqlQuery(queryString);
            ISparqlQueryResult result = model.ExecuteQuery(query, false);

            CreateSeriesPoints(result);

            _x.Minimum = DateTimeAxis.ToDouble(DateTime.Now.RoundToMinute().Subtract(TimeSpan.FromDays(2)));
            _x.Maximum = DateTimeAxis.ToDouble(DateTime.Now.RoundToMinute().AddDays(2));

            Model.ResetAllAxes();

            ResumeLayout();
        }
            
        private void CreateSeriesPoints(ISparqlQueryResult result)
        {
            AreaSeries series = CreateSeries("Facts x 1000", OxyColor.Parse("#119eda"));

            int d = 0;
            int n = 0;
            double y0 = 0;
            double y1 = 0;

            foreach (BindingSet binding in result.GetBindings())
            {
                DateTime x = DateTime.Parse(binding["time"].ToString());
                double y = Convert.ToInt32(binding["facts"]);

                if (n == 0)
                {
                    y0 = y;
                }

                if (d != x.DayOfYear)
                {
                    d = x.DayOfYear;

                    n++;
                }
                    
                y1 = y;

                series.Points.Add(DateTimeAxis.CreateDataPoint(x, y / 1000));
            }

            AverageDelta = n > 0 ? (y1 - y0) / n : 0;

            Model.Series.Add(series);
        }
          
        private AreaSeries CreateSeries(string title, OxyColor color)
        {
            OxyColor stroke = color;
            OxyColor fill = OxyColor.FromAColor(64, stroke);

            AreaSeries series = new AreaSeries();
            series.Title = title;
            series.Color = stroke;
            series.Color2 = fill;
            series.ConstantY2 = 0;
            series.StrokeThickness = 2;
            series.CanTrackerInterpolatePoints = true;
            series.Selectable = true;
            series.SelectionMode = OxyPlot.SelectionMode.Single;

            return series;
        }

        #endregion
    }
}

