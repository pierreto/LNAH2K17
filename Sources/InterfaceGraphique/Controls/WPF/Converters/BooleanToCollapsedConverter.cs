using System;
using System.Globalization;
using System.Windows;

namespace InterfaceGraphique.Controls.WPF.Converters
{
    class BooleanToCollapsedConverter : BaseValueConverter<BooleanToCollapsedConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return (bool)value ? Visibility.Collapsed : Visibility.Visible;
            else
                return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}