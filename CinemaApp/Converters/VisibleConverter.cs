using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace CinemaApp.Converters
{
    public class VisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool isFavorite)
            {
                if (parameter as string == "add")
                {
                    return isFavorite ? Visibility.Collapsed : Visibility.Visible;
                }

                return isFavorite ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
