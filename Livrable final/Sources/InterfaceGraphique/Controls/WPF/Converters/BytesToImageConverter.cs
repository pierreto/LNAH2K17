using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace InterfaceGraphique.Controls.WPF.Converters
{
    public class BytesToImageConverter : BaseValueConverter<BytesToImageConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value == null) return null;

            byte[] toBytes = Encoding.UTF8.GetBytes(value.ToString());

            /*var size = Marshal.SizeOf(value);
            // Both managed and unmanaged buffers required.
            var bytes = new byte[size];
            var ptr = Marshal.AllocHGlobal(size);
            // Copy object byte-to-byte to unmanaged memory.
            Marshal.StructureToPtr(value, ptr, false);
            // Copy data from unmanaged memory to managed buffer.
            Marshal.Copy(ptr, bytes, 0, size);
            // Release unmanaged memory.
            Marshal.FreeHGlobal(ptr);*/


            var image = new BitmapImage();
            try
            {
                using (MemoryStream mem = new MemoryStream(toBytes))
                {
                    mem.Position = 0;

                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = mem;
                    image.EndInit();
                }
                image.Freeze();
            }
            catch (Exception e)
            {
               byte[] test = System.IO.File.ReadAllBytes(Directory.GetCurrentDirectory() + "//media//image//coins.png");
            var image2 = new BitmapImage();

                BinaryFormatter bf = new BinaryFormatter();
                using (MemoryStream mem = new MemoryStream(test))
                {
                   // bf.Serialize(mem, test);
                    //int offset = 78;
                    //mem.Write(test, offset, test.Length - offset);

                    mem.Position = 0;
                    image2.BeginInit();
                    image2.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image2.CacheOption = BitmapCacheOption.OnLoad;
                    image2.UriSource = null;
                    image2.StreamSource = mem;
                    image2.EndInit();
                }
                image2.Freeze();
                    //image = new BitmapImage(new Uri(Directory.GetCurrentDirectory()+"//media//image//coins.png"));

                image = image2;
            }

            return image;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
