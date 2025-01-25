using System;
using Microsoft.Maui.Controls;
using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Resources.Converters
{
    public class DiscountTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var promotionCard = value as PromotionCard;

            if (promotionCard != null)  // Null-safe check for PromotionCard
            {
                var discountType = promotionCard.DiscountType?.ToLower();  // Null-safe check for DiscountType
                var discountAmount = promotionCard.DiscountAmount;

                if (discountType == "percentage")
                {
                    return $"-{discountAmount}%";
                }
                else if (discountType == "amount")
                {
                    return $"-{discountAmount:C}";  // Formats as currency
                }
            }
            return string.Empty;  // Return an empty string if no valid data found
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
