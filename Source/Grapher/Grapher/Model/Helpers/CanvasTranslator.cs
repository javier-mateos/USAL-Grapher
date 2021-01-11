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

        public static double XScreenToXReal(double xScreen, double xMin, double xMax, double canvasWidth)
        {
            return ((xMax - xMin) * xScreen / canvasWidth) + xMin;
        }

        public static double YScreenToYReal(double yScreen, double yMin, double yMax, double canvasHeight)
        {
            return ((yMax - yMin) * ((yScreen - canvasHeight) / canvasHeight) + yMin);
        }
    }
}
