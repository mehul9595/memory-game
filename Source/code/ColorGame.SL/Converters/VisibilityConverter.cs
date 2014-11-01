using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ColorGame.SL.Converters
{
    /// <summary>
    ///     Handles the visibility any control based on boolean value.
    /// </summary>
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Visibility.Visible;

            if ((bool) value)
                return Visibility.Collapsed;
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}