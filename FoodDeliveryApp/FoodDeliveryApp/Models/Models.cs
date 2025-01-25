using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Models
{
    public class Models
    {
        public string Message { get; set; }
        public int Id_Users { get; set; } // This should match the JSON response structure
    }
    public class MostOrderedCategory
    {
        public int Id_Categories { get; set; }
        public string CategoryName { get; set; }
        public int OrderCount { get; set; }
    }
    public class RestaurantDetails
    {
        public int Id { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantEmail { get; set; }
        public string RestaurantPhoneNumber { get; set; }
        public string RestaurantAddress { get; set; }
        public string RestaurantCity { get; set; }
        public string RestaurantCountry { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreatedAt { get; set; }

        // Nullable properties for menu and category (optional if not used immediately)
        public object Menu { get; set; }
        public object Category { get; set; }
    }
    public class CartItem
    {
        public int Id_FoodItems { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
    public class FoodItemWithRestaurantDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int RestaurantId { get; set; } // New property
        public string RestaurantName { get; set; }
        public string RestaurantLocation { get; set; }
        public int Quantity { get; set; } // New property
    }
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class ChangePasswordRequest
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
    public class CartRequest
    {
        public int Id_Users { get; set; } // Retrieve this from Preferences
        public List<CartItem> CartItems { get; set; }
    }
    public class OrderResponse
    {
        public string Message { get; set; }
        public int Id_Orders { get; set; }
    }
    public class FoodItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        private double _ratings;  // Backing field
        public double Ratings
        {
            get
            {
                // Round to 1 decimal place when retrieving the value
                return Math.Round(_ratings, 1);
            }
            set
            {
                // Always round to 1 decimal place when setting the value
                _ratings = Math.Round(value, 1);
            }
        }
public int NumOfReviews { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreatedAt { get; set; }
        public RestaurantDto Restaurant { get; set; }
    }

    public class RestaurantDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class CategoryDto
    {
        public int Id_Categories { get; set; }
        public string CategoryName { get; set; }
    }
    public class FoodItemsSearchQuery
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string RestaurantName { get; set; }
    }
    public class RegionCode
    {
        public string Code { get; set; }  // Example: "+33"
        public string Emoji { get; set; } // Example: "🇫🇷"
        public string DisplayText => $"{Emoji} {Code}";
    }
    public class PaginatedResponse<T>
    {
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public List<T> Items { get; set; }
    }
    public class RatingRequest
    {
        public int UserId { get; set; }
        public int FoodItemId { get; set; }
        public int Rating { get; set; }
    }
    public class Menu
    {
        public int Id { get; set; }
        public int Id_Restaurants { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation Property
        public RestaurantDto Restaurant { get; set; }
        public ObservableCollection<FoodItemDto> FoodItems { get; set; }
    }
    public class UserCards
    {
        public int Id { get; set; }
        public bool IsUsed { get; set; }
        public int Id_Users { get; set; }
        public int Id_PromotionCards { get; set; }

        public virtual PromotionCard PromotionCard { get; set; }
    }
    public class PromotionCard
    {
        public int Id { get; set; }
        public string CardCode { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal MinAmount { get; set; }
        public string DiscountType { get; set; }
    }

}