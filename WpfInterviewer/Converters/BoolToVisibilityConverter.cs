using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfInterviewer.Converters
{
	public class BoolToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object result;
			if (null != value)
			{
				bool boolVal = true;
				if (bool.TryParse(value.ToString(), out boolVal))
				{
					result = (boolVal ? Visibility.Collapsed : Visibility.Visible);
					return result;
				}
			}
			result = Visibility.Visible;
			return result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
