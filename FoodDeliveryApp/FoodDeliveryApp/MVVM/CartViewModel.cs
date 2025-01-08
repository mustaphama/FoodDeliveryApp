using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Popups;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace FoodDeliveryApp.MVVM
{
    public partial class CartViewModel : ObservableObject
    {
        private readonly CartService _cartService;
        private readonly ApiService _apiService;

        public ObservableCollection<FoodItemWithRestaurantDto> CartItems { get; set; } = new();
        public decimal TotalPrice => CartItems.Sum(item => item.Price * item.Quantity);
        public ICommand NavigateToDetailsCommand { get; }
        public ICommand CheckoutCommand { get; }

        // Constructor with DI for Cart Service
        public CartViewModel(CartService cartService)
        {
                _cartService = cartService;
                _apiService = new ApiService();
                LoadCartCommand = new Command(async () => await LoadCartItems());
                NavigateToDetailsCommand = new AsyncRelayCommand<FoodItemWithRestaurantDto>(NavigateToDetailsAsync);
                CheckoutCommand = new AsyncRelayCommand(CheckoutAsync);
        }

        public ICommand LoadCartCommand { get; }

        private async Task LoadCartItems()
        {
            var OrderId = Preferences.Get("CurrentOrderId", 0);
            if (OrderId != 0)
            {
                await Shell.Current.GoToAsync("OrderStatusPage");
            }
            var cart = _cartService.GetCartItems();

            // Extract food item IDs from the cart
            var foodItemIds = cart.Select(item => item.FoodItemId).ToList();

            if (foodItemIds.Count > 0)
            {
                // Fetch details for the cart items
                var foodItems = await _apiService.GetFoodItemsByIdsAsync(foodItemIds);

                // Map quantities to the retrieved items
                foreach (var item in foodItems)
                {
                    var cartItem = cart.FirstOrDefault(c => c.FoodItemId == item.Id);
                    if (cartItem != null)
                    {
                        item.Quantity = cartItem.Quantity;
                    }
                    Debug.WriteLine($"CartItems name: {item.Name}");
                }

                // Update the observable collection
                CartItems.Clear();
                foreach (var item in foodItems)
                {
                    CartItems.Add(item);
                }

                // Notify UI to refresh bindings
                OnPropertyChanged(nameof(CartItems));
                OnPropertyChanged(nameof(TotalPrice));
            }
        }

        public ICommand IncreaseQuantityCommand => new Command<FoodItemWithRestaurantDto>(async item =>
        {
            if (item.Quantity < 99)
            {
                _cartService.AddOrUpdateCartItem(item.Id, 1, item.Price);
                item.Quantity++;
                OnPropertyChanged(nameof(TotalPrice));
                await ReloadCart();
                Debug.WriteLine($"Added one.");
            }
        });

        public ICommand DecreaseQuantityCommand => new Command<FoodItemWithRestaurantDto>(async item =>
        {
            if (item.Quantity > 1)
            {
                _cartService.AddOrUpdateCartItem(item.Id, -1, item.Price);
                item.Quantity--;
                OnPropertyChanged(nameof(TotalPrice));
                await ReloadCart();
                Debug.WriteLine($"Removed one.");
            }
            else
            {
                // Show custom popup
                var popup = new RemoveItemPopup(item.Name, result =>
                {
                    if (result)
                    {
                        // Remove the item from the cart
                        _cartService.RemoveCartItem(item.Id);
                        CartItems.Remove(item);
                        OnPropertyChanged(nameof(TotalPrice));
                    }
                });

                App.Current.MainPage.ShowPopup(popup);
            }
        });
        private async Task CheckoutAsync()
        {
            // Navigate to the checkout page
            await Shell.Current.GoToAsync("DeliveryDetailsPage");
        }
        private async Task NavigateToDetailsAsync(FoodItemWithRestaurantDto product)
        {
            try
            {
                if (product != null)
                {
                    var navigationParameter = new Dictionary<string, object>
        {
            { "SelectedProductID", product.Id.ToString() }
        };
                    await Shell.Current.GoToAsync("ProductDetailsPage", true, navigationParameter);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        private async Task ReloadCart()
        {
            // Refresh the cart by invoking the load command
            await LoadCartItems();
        }
    }
}
