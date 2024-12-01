using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Crematorium.UI.Converters
{
    public class ConverterBytesToImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value as byte[] == null)
                return null;

            byte[] array = (byte[])value;
            MemoryStream memorystream = new MemoryStream();
            memorystream.Write(array, 0, array.Length);
            memorystream.Seek(0, SeekOrigin.Begin);
            BitmapImage imgsource = new BitmapImage();
            //imgsource.BeginInit();
            using (imgsource.StreamSource = memorystream)
            //imgsource.EndInit();
            return imgsource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
