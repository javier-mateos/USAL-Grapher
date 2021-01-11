using System;
using System.Collections.ObjectModel;

namespace Grapher
{
    class MathCalc
    {
        static public void PolynomialCalculator(Graph graph, double canvasWidth)
        {
            ObservableCollection<Point2D> temp = new ObservableCollection<Point2D>();

            graph.Points.Clear();

            for (int i = 0; i < canvasWidth; i++)
            {
                double res = 0;
                double xValue = graph.PolyExp.XMinVal + i * (graph.PolyExp.XMaxVal - graph.PolyExp.XMinVal) / canvasWidth;

                foreach (MonomialMember mon in graph.PolyExp.Monomials)
                    res += mon.Value * Math.Pow(xValue, mon.Grade);

                temp.Add(new Point2D {XValue = xValue, YValue = res});
            }

            graph.Points = temp;
        }
    }
}
