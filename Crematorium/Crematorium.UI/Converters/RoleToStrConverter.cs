using Crematorium.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Crematorium.UI.Converters
{
    internal class RoleToStrConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            value = (Role)value;
            switch (value)
            {
                case Role.Admin:
                    return "Admin";
                case Role.Employee:
                    return "Employee";
                case Role.Customer:
                    return "Customer";
                case Role.NoName:
                    return "No role";
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
