using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ApiTester.Client.Converters
{
    public class StringToBrushColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (null != value)
            {
                var color = value.ToString();
                return new SolidColorBrush(Color.FromArgb(
                     Byte.Parse(color.Substring(1, 2), NumberStyles.HexNumber),
                     Byte.Parse(color.Substring(3, 2), NumberStyles.HexNumber),
                     Byte.Parse(color.Substring(5, 2), NumberStyles.HexNumber),
                     Byte.Parse(color.Substring(7, 2), NumberStyles.HexNumber)
                    ));
            }

            return new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
