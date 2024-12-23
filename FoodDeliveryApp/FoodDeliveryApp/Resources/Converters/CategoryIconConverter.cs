using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace FoodDeliveryApp.Resources.Converters
{
    public class CategoryIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string categoryName)
            {
                return $"/categoriesIcons/{categoryName.Replace(" ","_").ToLower()}.png";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
