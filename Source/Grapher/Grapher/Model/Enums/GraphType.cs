using System.ComponentModel;

namespace Grapher
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    enum GraphType
    {
        [Description("Line Graph")]
        LineGraph,

        [Description("Bar Graph")]
        BarGraph
    }

}
