using System;
using System.Globalization;
using System.Windows.Data;

namespace Complaints_WPF.Converter
{
    public class ZeroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.Equals(0))
                return string.Empty;
            else
                return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.Equals(string.Empty))
                return 0;
            else
                return value;
        }
    }
}
