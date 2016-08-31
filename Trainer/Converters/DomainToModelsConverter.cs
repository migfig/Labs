using System;
using Trainer.Domain;
using Windows.UI.Xaml.Data;
using Trainer.Models;
using Windows.UI.Xaml;
using Log.Common.Services.Common;
using Log.Common.Services;

namespace Trainer.Converters
{
    public class DomainToModelsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(value != null)
            {
                var isThumbnail = false;
                if (null != parameter)
                {
                    bool.TryParse(parameter.ToString(), out isThumbnail);
                }

                if (value is RichTextBlock)
                { 
                    return ((RichTextBlock)value).Control(isThumbnail);
                }
                else if(value is Component)
                {
                    return ((Component)value).Control(isThumbnail);
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

    public class SlideToMarkdownConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null && value is Slide)
            {
                using (var service = ApiServiceFactory.CreateService<string>(Services.SettingsServices.SettingsService.Instance.CodeServicesUrl))
                {
                    var parser = ParserFactory<Slide>.CreateSlideParser(service);
                    return parser.Parse(value as Slide);
                }
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
