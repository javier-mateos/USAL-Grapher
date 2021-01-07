using System;
using System.Globalization;
using System.Windows.Controls;

namespace Grapher
{
    class StringToDouble : ValidationRule
    {
        public int Max { get; set; }
        public int Min { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            double res = 0;

            if (((string)value).EndsWith(".") || ((string)value).StartsWith(".") || !double.TryParse((string)value, NumberStyles.Float, CultureInfo.InvariantCulture, out res) || (res < Min) || (res > Max))
                return new ValidationResult(false, $"Enter a valid Double between {Min} and {Max}");

            return ValidationResult.ValidResult;
        }
    }
}
