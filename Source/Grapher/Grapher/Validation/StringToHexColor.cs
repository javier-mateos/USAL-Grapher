using System;
using System.Globalization;
using System.Windows.Controls;

namespace Grapher
{
    class StringToHexColor : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string colorText = ((string)value);

            try
            {
                if (colorText.Length == 7 && colorText.StartsWith("#"))
                {
                    int.Parse(colorText.Substring(1), NumberStyles.HexNumber);
                    return ValidationResult.ValidResult;
                }
                else
                    return new ValidationResult(false, $"Enter a Hexadecimal Color (#XXXXXX)");
            }
            catch (Exception)
            {
                return new ValidationResult(false, $"Enter a Hexadecimal Color (#XXXXXX)");
            }
        }
    }
}
