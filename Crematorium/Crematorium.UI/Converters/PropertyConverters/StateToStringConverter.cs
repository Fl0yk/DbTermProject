using Crematorium.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Crematorium.UI.Converters.PropertyConverters
{
    public class StateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            value = (StateOrder)value;
            switch (value)
            {
                case StateOrder.Decorated:
                    return "Decorated";
                case StateOrder.Approved:
                    return "Approved";
                case StateOrder.Closed:
                    return "Closed";
                case StateOrder.Cancelled:
                    return "Canceled";
                default:
                    throw new NotImplementedException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
