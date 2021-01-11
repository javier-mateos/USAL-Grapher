using System;
using System.ComponentModel;

namespace Grapher
{
    class CanvasData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public double xMin { get; set; } = double.NaN;
        public double xMax { get; set; } = double.NaN;
        public double yMin { get; set; } = double.NaN;
        public double yMax { get; set; } = double.NaN;
        public Point2D SelectionStart { get; set; }
        public Point2D SelectionEnd { get; set; }

        public CanvasData()
        {
            /* Selection Points */
            SelectionStart = new Point2D { XValue = double.NaN, YValue = double.NaN };
            SelectionEnd = new Point2D { XValue = double.NaN, YValue = double.NaN };
        }
    }
}
