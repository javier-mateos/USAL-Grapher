using System;
using System.Collections.ObjectModel;

namespace Grapher
{
    class MathCalc
    {
        static public void PolynomialCalculator(PolynomialExpression poly, double canvasWidth)
        {

            poly.CalculatedPoints.Clear();

            for (int i = 0; i < canvasWidth; i++)
            {
                double res = 0;
                double xValue = poly.XMinVal + i * (poly.XMaxVal - poly.XMinVal) / canvasWidth;

                foreach (MonomialMember mon in poly.Monomials)
                    res += mon.Value * Math.Pow(xValue, mon.Grade);

                poly.CalculatedPoints.Add(new Point2D {XValue = xValue, YValue = res});
            }

        }
    }
}
