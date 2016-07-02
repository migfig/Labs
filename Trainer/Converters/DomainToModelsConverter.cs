using System;
using Trainer.Domain;
using Windows.UI.Xaml.Data;
using Trainer.Models;
using Windows.UI.Xaml;

namespace Trainer.Converters
{
    public class DomainToModelsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(value != null)
            {
                if(value is RichTextBlock)
                {
                    return ((RichTextBlock)value).Control();
                }
                else if(value is Component)
                {
                    return ((Component)value).Control();
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class StringToMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(value != null && !string.IsNullOrEmpty(value.ToString()))
            {
                return Helpers.GetThickness(value.ToString());
            }

            return new Thickness(0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
