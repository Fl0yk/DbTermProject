using System;
using System.Globalization;
using System.Windows.Data;

namespace Crematorium.UI.Converters.PropertyConverters
{
    public class DateOfStartToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime date)
            {
                return date.ToString("dd.MM.yyyy");
            }

            return value?.ToString() ?? "";
            //var date = value;
            //if(date is null)
            //    return string.Empty;

            //return date.Data;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
