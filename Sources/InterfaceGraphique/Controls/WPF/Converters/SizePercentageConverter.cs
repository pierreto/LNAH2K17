using System;
using System.Globalization;
using System.Windows.Data;

namespace InterfaceGraphique.Controls.WPF.Converters
{
    public class SizePercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                if (value != null) return 0.5 * (double) value;

            string[] split = parameter.ToString().Split('.');
            double parameterDouble = double.Parse(split[0]) + double.Parse(split[1]) / (Math.Pow(10, split[1].Length));
            return (double) value * parameterDouble;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Don't need to implement this
            return null;
        }
    }
}
