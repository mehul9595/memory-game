using System;
using System.Globalization;
using System.Windows.Data;

namespace ColorGame.SL.Converters
{
    public class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "00:00";
            var time = (int) value;
            int minutes = time/60;
            int secs = time%60;
            return String.Format("{0}:{1}", minutes.ToString("D2"), secs.ToString("D2"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}