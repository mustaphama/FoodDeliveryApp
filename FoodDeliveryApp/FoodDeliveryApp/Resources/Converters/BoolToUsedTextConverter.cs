using System;
using Microsoft.Maui.Controls;

namespace FoodDeliveryApp.Resources.Converters
{
    public class BoolToUsedTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isUsed = (bool)value;
            return isUsed ? "Used" : "Available";  // Shows "Used" when IsUsed is true
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
