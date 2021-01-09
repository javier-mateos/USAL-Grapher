using System.Globalization;
using System.Windows.Controls;

namespace Grapher
{
    class StringToInt : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int res;
            string number = (string)value;

            return (int.TryParse(number, out res) ? ValidationResult.ValidResult : new ValidationResult(false, $"Enter a valid Integer"));
        }
    }
}
