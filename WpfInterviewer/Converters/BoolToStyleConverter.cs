using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfInterviewer.Converters
{
	public class BoolToStyleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object result;
			if (null != value)
			{
				bool boolVal = true;
				if (bool.TryParse(value.ToString(), out boolVal))
				{
					if (boolVal)
					{
						result = (Application.Current.Resources["SucceededRadioButton"] as Style);
						return result;
					}
					result = (Application.Current.Resources["FailedRadioButton"] as Style);
					return result;
				}
			}
			result = (Application.Current.Resources["DefaultRadioButton"] as Style);
			return result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
