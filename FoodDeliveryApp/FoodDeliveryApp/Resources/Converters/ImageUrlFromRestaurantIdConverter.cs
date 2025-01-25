using System;
using System.Globalization;

namespace FoodDeliveryApp.Resources.Converters
{
    public class ImageUrlFromRestaurantIdConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int id)
            {
                // Generate the URL dynamically based on the ID
                return $"/restaurant_logos/image_{id}.png";
            }
            return null; // Return null if the value is not valid
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
