namespace Grapher
{
    class CanvasTranslator
    {
        public static double XRealToXScreen(double xReal, double xMin, double xMax, double canvasWidth)
        {
            return canvasWidth * ((xReal - xMin) / (xMax - xMin));
        }

        public static double YRealToYScreen(double yReal, double yMin, double yMax, double canvasHeight)
        {
            return ((canvasHeight * (yMax - yReal)) / (yMax - yMin));
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
