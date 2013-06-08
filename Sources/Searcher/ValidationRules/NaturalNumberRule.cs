using System;
using System.Globalization;
using System.Windows.Controls;

namespace Searcher.ValidationRules
{
    public class NaturalNumberRule : ValidationRule
    {

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int num = 0;

            if (!int.TryParse(value.ToString(), out num))
                return new ValidationResult(false, String.Format("Value '{0}' is not a number!", value));

            if (num < 0)
                return new ValidationResult(false, "Only positive numbers are valid!");
            return  new ValidationResult(true, null);

        }
    }
}
