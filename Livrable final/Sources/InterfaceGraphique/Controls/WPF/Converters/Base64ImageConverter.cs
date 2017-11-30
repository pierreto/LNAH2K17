using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace InterfaceGraphique.Controls.WPF.Converters
{
    public class Base64ImageConverter : BaseValueConverter<Base64ImageConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            BitmapImage bi = new BitmapImage();

            try
            {
                string s = value as string;

                if (s == null || s == "")
                {
                    return Directory.GetCurrentDirectory() + "\\media\\image\\default_profile_picture.png";
                }


                bi.BeginInit();
                bi.StreamSource = new MemoryStream(System.Convert.FromBase64String(s));
                bi.EndInit();
            }
            catch (Exception e)
            {
                return Directory.GetCurrentDirectory() + "\\media\\image\\default_profile_picture.png";
            }


            return bi;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
