using System.ComponentModel;

namespace Grapher
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    enum GraphType
    {
        [Description("Polynomial Graph")]
        Polynomial,

        [Description("Line Graph")]
        LineGraph,

        [Description("Bar Graph")]
        BarGraph
    }

}
