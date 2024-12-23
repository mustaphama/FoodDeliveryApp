using FoodDeliveryApp.Models;
using Microsoft.Maui.Storage;
using System.Text.Json;

public class CartService
{
    private const string CartKey = "CartData";
    private readonly List<CartItem> _cartItems;

    public CartService()
    {
        _cartItems = LoadCartFromPreferences();
    }

    public IReadOnlyList<CartItem> GetCartItems() => _cartItems.AsReadOnly();

    public void AddOrUpdateCartItem(int foodItemId, int quantity, decimal price)
    {
        var existingItem = _cartItems.FirstOrDefault(item => item.FoodItemId == foodItemId);

        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            _cartItems.Add(new CartItem
            {
                FoodItemId = foodItemId,
                Quantity = quantity,
                Price = price
            });
        }
        SaveCartToPreferences();
    }
    public void RemoveCartItem(int foodItemId)
    {
        var itemToRemove = _cartItems.FirstOrDefault(c => c.FoodItemId == foodItemId);
        if (itemToRemove != null)
        {
            _cartItems.Remove(itemToRemove);
        }
    }
        public int GetCountCart()
    {
        return _cartItems.Count;
    }
    private void SaveCartToPreferences()
    {
        var json = JsonSerializer.Serialize(_cartItems);
        Preferences.Set(CartKey, json);
    }
    private List<CartItem> LoadCartFromPreferences()
    {
        if (Preferences.ContainsKey(CartKey))
        {
            var json = Preferences.Get(CartKey, string.Empty);
            return JsonSerializer.Deserialize<List<CartItem>>(json) ?? new List<CartItem>();
        }
        return new List<CartItem>();
    }
}