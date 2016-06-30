using System;
using Trainer.Domain;
using Windows.UI.Xaml.Data;
using Trainer.Models;

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
}
