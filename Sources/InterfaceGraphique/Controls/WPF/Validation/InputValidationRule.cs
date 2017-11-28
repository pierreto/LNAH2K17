using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace InterfaceGraphique.Controls.WPF.Validation
{
    public class InputValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string champ = GetBoundValue(value) as String;
            if (string.IsNullOrWhiteSpace(champ))
            {
                return new ValidationResult(false, "Le champ ne peut être vide");
            }
            Regex regex = new Regex(@"^[a-zA-Z0-9]*$");
            Match match = regex.Match(champ);
            if (!match.Success)
            {
                return new ValidationResult(false, "Le champ doit utiliser des charactères valide (A-Z et 0-9)");
            }
            if (champ.Length >127)
            {
                return new ValidationResult(false, "Le champ ne peut dépasser 127 charactères");
            }
         
            return ValidationResult.ValidResult;
        }

        public static bool ValidateInput(string item)
        {

            if (string.IsNullOrWhiteSpace(item))
            {
                return false;
            }
            Regex regex = new Regex(@"^[a-zA-Z0-9]*$");
            Match match = regex.Match(item);
            if (!match.Success)
            {
                return false;
            }
             if (item.Length > 127)
            {
                return false;
            }
            return true;
        }

        private object GetBoundValue(object value)
        {
            if (value is BindingExpression)
            {
                // ValidationStep was UpdatedValue or CommittedValue (Validate after setting)
                // Need to pull the value out of the BindingExpression.
                BindingExpression binding = (BindingExpression)value;

                // Get the bound object and name of the property
                object dataItem = binding.DataItem;
                string propertyName = binding.ParentBinding.Path.Path;

                // Extract the value of the property.
                object propertyValue = dataItem.GetType().GetProperty(propertyName).GetValue(dataItem, null);

                // This is what we want.
                return propertyValue;
            }
            else
            {
                // ValidationStep was RawProposedValue or ConvertedProposedValue
                // The argument is already what we want!
                return value;
            }
        }
    }
}
