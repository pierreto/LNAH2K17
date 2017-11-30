using System;
using System.Globalization;
using System.Windows;

namespace InterfaceGraphique.Controls.WPF.Converters
{
    class StringToVisibilityConverter : BaseValueConverter<StringToVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return ((string)value != "" && (string)value != null) ? Visibility.Collapsed : Visibility.Visible;
            else
                return ((string)value != "" && (string)value != null) ? Visibility.Visible : Visibility.Collapsed;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}