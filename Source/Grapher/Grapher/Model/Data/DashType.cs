using System.ComponentModel;

namespace Grapher
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    enum DashType
    {
        [Description("None")]
        None,

        [Description("Dotted")]
        Dot,

        [Description("Dotted Plus")]
        DottedPlus,

        [Description("Dashed")]
        Dashed,

        [Description("Dot Dash")]
        DotDash,

        [Description("Dot Dot Dash")]
        DotDotDash
    }

}