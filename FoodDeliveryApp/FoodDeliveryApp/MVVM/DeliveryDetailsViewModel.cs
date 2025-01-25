using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using FoodDeliveryApp.Models;
using System.Windows.Input;
using System.Diagnostics;

namespace FoodDeliveryApp.MVVM
{
    public partial class DeliveryDetailsViewModel : ObservableObject
    {
        private readonly CartService _cartService;
        private readonly ApiService _apiService;

        public ObservableCollection<FoodItemWithRestaurantDto> CartItems { get; set; } = new();

        public ICommand LoadCartCommand { get; }

        public DeliveryDetailsViewModel(CartService cartService)
        {
            _apiService = new ApiService();
            _cartService = cartService;
            LoadCartCommand = new Command(async () => await LoadCartItems());

        }
        [ObservableProperty]
        private decimal _totalPrice = 0;
        [ObservableProperty]
        private decimal _deliveryFee = 0;
        [ObservableProperty]
        private decimal _total = 0;
        private async Task LoadCartItems()
        {
            var cart = _cartService.GetCartItems();

            // Extract food item IDs from the cart
            var foodItemIds = cart.Select(item => item.Id_FoodItems).ToList();

            if (foodItemIds.Count > 0)
            {
                // Fetch details for the cart items
                var foodItems = await _apiService.GetFoodItemsByIdsAsync(foodItemIds);

                // Map quantities to the retrieved items
                foreach (var item in foodItems)
                {
                    var cartItem = cart.FirstOrDefault(c => c.Id_FoodItems == item.Id);
                    if (cartItem != null)
                    {
                        item.Quantity = cartItem.Quantity;
                        item.Price = cartItem.Price* item.Quantity;
                    }
                    Debug.WriteLine($"CartItems name: {item.Name}");
                }
                TotalPrice = cart.Sum(item => item.Price * item.Quantity);

                // Calculate delivery fee based on total price and quantity (customize the logic as needed)
                DeliveryFee = CalculateDeliveryFee(TotalPrice, cart.Sum(item => item.Quantity));

                Total = TotalPrice + DeliveryFee;

                // Update the observable collection
                CartItems.Clear();
                foreach (var item in foodItems)
                {
                    CartItems.Add(item);
                }

                // Notify UI to refresh bindings
                OnPropertyChanged(nameof(CartItems));
            }
        }
        private decimal CalculateDeliveryFee(decimal totalPrice, int totalQuantity)
        {
            // Example logic: Delivery fee is 5% of the total price + $2 per item
            decimal deliveryFeePercentage = 0.05m;
            decimal feePerItem = 1m;

            decimal feeBasedOnPrice = totalPrice * deliveryFeePercentage;
            decimal feeBasedOnQuantity = feePerItem * totalQuantity;

            // You can adjust this logic based on your business rules
            return feeBasedOnPrice + feeBasedOnQuantity;
        }
        // Command to modify the delivery address
        [RelayCommand]
        private async Task ModifyAddressAsync()
        {
            // Logic to navigate to modify address page
            await Shell.Current.GoToAsync("ModifyAddressPage");
        }

        // Command to change the phone number
        [RelayCommand]
        private async Task ChangePhoneNumberAsync()
        {
            // Logic to navigate to change phone number page
            await Shell.Current.GoToAsync("ModifyPhonePage");
        }

        // Command to add a coupon
        [RelayCommand]
        private async Task AddCouponAsync()
        {
            // Logic to navigate to add coupon page
            await Shell.Current.GoToAsync("AddCouponPage");
        }

        // Command to proceed to checkout
        [RelayCommand]
        private async Task CheckoutAsync()
        {
            if (!CartItems.Any())
            {
                Debug.WriteLine("Cart is empty!");
                return;
            }

            // Create the order request
            var cartRequest = new CartRequest
            {
                Id_Users = Preferences.Get("UserId", 0), // Replace with your user preference key
                CartItems = CartItems.Select(item => new CartItem
                {
                    Id_FoodItems = item.Id,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            };

            // Call the API to place the order
            var (isSuccess, errorMessage, orderId) = await _apiService.PlaceOrderAsync(cartRequest);

            if (isSuccess && orderId > 0)
            {
                Debug.WriteLine($"Order placed successfully! Order ID: {orderId}");

                // Save the Id_Orders in preferences
                Preferences.Set("CurrentOrderId", orderId);

                // Navigate to a confirmation page or clear the cart
                _cartService.ClearCart();
                CartItems.Clear();
                OnPropertyChanged(nameof(CartItems));

                await Shell.Current.GoToAsync("OrderStatusPage");
            }
            else
            {
                Debug.WriteLine($"Failed to place order. Error: {errorMessage}");
                // Display an error message to the user, if needed
            }
        }
    }
}
