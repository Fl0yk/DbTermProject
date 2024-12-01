using Crematorium.Domain.Entities;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Crematorium.UI.Converters.VisibleConverters
{
    internal class CustomerVisiblility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var user = (User)value;

            if (user is null)
                return Visibility.Hidden;

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
