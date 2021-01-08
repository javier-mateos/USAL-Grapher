using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grapher
{
    class CanvasTransalator
    {
        public static double XRealToXScreen(double xReal, double xMin, double xMax, double canvasWidth)
        {
            return canvasWidth * ((xReal - xMin) / (xMax - xMin));
        }

        public static double XRealToYScreen(double yReal, double yMin, double yMax, double canvasHeight)
        {
            return canvasHeight * (1 - ((yReal - yMin) / (yMax - yMin)));
        }

        /*public static double XScreenToXReal(double x, double Width, RepresentationParameters RepresentationParameters)
        {
            return ((RepresentationParameters.XMax - RepresentationParameters.XMin) * x / Width) + RepresentationParameters.XMin;
        }

        public static double YScreenToYReal(double y, double Height, RepresentationParameters RepresentationParameters)
        {
            return RepresentationParameters.YMin - ((RepresentationParameters.YMax - RepresentationParameters.YMin) * (y - Height) / Height);
        }*/
    }
}
