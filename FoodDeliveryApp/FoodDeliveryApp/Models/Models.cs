﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Models
{
    public class Models
    {
        public string Message { get; set; }
        public int UserId { get; set; } // This should match the JSON response structure
    }
    public class MostOrderedCategory
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int OrderCount { get; set; }
    }
    public class HotProduct
    {
        public int foodItemId { get; set; }
        public string foodItemName { get; set; }
        public string Price { get; set; }
        public string ImageUrl { get; set; }
        public string description { get; set; }
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
        public int FoodItemId { get; set; }
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


}