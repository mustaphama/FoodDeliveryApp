using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Timers;
using System.Windows.Input;

namespace FoodDeliveryApp.MVVM
{
    public partial class OrderStatusViewModel : ObservableObject
    {
        private readonly ApiService _apiService;
        private readonly System.Timers.Timer _statusCheckTimer;

        [ObservableProperty]
        private string _orderStatus;  // Internal status for logic

        [ObservableProperty]
        private string _visualOrderStatus; // User-friendly status for display

        [ObservableProperty]
        private double _orderProgress;  // Progress values (0.0 to 1.0 for ProgressBar)

        [ObservableProperty]
        private bool _isRefreshing; // Flag to handle refreshing

        private int _currentOrderId;

        public ICommand CheckOrderStatusCommand { get; }

        public OrderStatusViewModel()
        {
            _apiService = new ApiService();
            _currentOrderId = Preferences.Get("CurrentOrderId", 0); // Retrieve current order ID
            CheckOrderStatusCommand = new AsyncRelayCommand(CheckOrderStatusAsync);

            OrderStatus = "Pending"; // Default initial status
            VisualOrderStatus = "Order placed and awaiting confirmation.";
            OrderProgress = 0.0;

            _statusCheckTimer = new System.Timers.Timer(5000); // Check every 5 seconds
            _statusCheckTimer.Elapsed += async (s, e) => await CheckOrderStatusAsync();
            _statusCheckTimer.Start();
        }

        private async Task CheckOrderStatusAsync()
        {
            if (_currentOrderId == 0)
            {
                OrderStatus = "No order placed.";
                VisualOrderStatus = "You have no active orders.";
                OrderProgress = 0.0;
                StopTimer();
                return;
            }

            try
            {
                IsRefreshing = true;  // Set the refreshing flag

                var statusResponse = await _apiService.GetOrderStatusAsync(_currentOrderId);

                if (statusResponse.IsSuccess)
                {
                    OrderStatus = statusResponse.Status;
                    UpdateProgressAndVisualStatus(OrderStatus);
                    Debug.WriteLine($"Order status updated: {OrderStatus}");

                    if (OrderStatus == "Completed" || OrderStatus == "Cancelled")
                    {
                        // Clear current order and redirect
                        Preferences.Set("CurrentOrderId", 0);
                        StopTimer();
                        await Shell.Current.GoToAsync("OldOrdersPage");
                    }
                }
                else
                {
                    VisualOrderStatus = $"Error fetching status: {statusResponse.ErrorMessage}";
                }

                IsRefreshing = false;  // Reset refreshing flag
            }
            catch (Exception ex)
            {
                VisualOrderStatus = $"Error: {ex.Message}";
                Debug.WriteLine(VisualOrderStatus);
                IsRefreshing = false;  // Reset refreshing flag
            }
        }

        [ObservableProperty]
        private string _statusImageSource; // Path to the GIF for the current status

        private void UpdateProgressAndVisualStatus(string status)
        {
            switch (status)
            {
                case "Pending":
                    OrderProgress = 0.1;
                    VisualOrderStatus = "Order placed and awaiting confirmation.";
                    StatusImageSource = "orderStatus_gifs/pending.gif"; // Replace with your actual GIF file name
                    break;
                case "Confirmed":
                    OrderProgress = 0.3;
                    VisualOrderStatus = "Your order has been confirmed.";
                    StatusImageSource = "orderStatus_gifs/confirmed.gif";
                    break;
                case "Preparing":
                    OrderProgress = 0.5;
                    VisualOrderStatus = "Preparing your food.";
                    StatusImageSource = "orderStatus_gifs/preparing.gif";
                    break;
                case "Ready for Pickup":
                    OrderProgress = 0.7;
                    VisualOrderStatus = "Your order is ready for pickup.";
                    StatusImageSource = "orderStatus_gifs/ready_for_pickup.gif";
                    break;
                case "Out for Delivery":
                    OrderProgress = 0.9;
                    VisualOrderStatus = "The driver is on the way.";
                    StatusImageSource = "orderStatus_gifs/out_for_delivery.gif";
                    break;
                case "Delivered":
                case "Completed":
                    OrderProgress = 1.0;
                    VisualOrderStatus = "Order successfully delivered. Bon appétit!";
                    StatusImageSource = "orderStatus_gifs/delivered.gif";
                    break;
                case "Cancelled":
                    OrderProgress = 0.0;
                    VisualOrderStatus = "Your order has been cancelled.";
                    StatusImageSource = "orderStatus_gifs/cancelled.gif";
                    break;
                default:
                    OrderProgress = 0.0;
                    VisualOrderStatus = "Unknown status.";
                    StatusImageSource = "orderStatus_gifs/cancelled.gif";
                    break;
            }
        }

        private void StopTimer()
        {
            if (_statusCheckTimer != null)
            {
                _statusCheckTimer.Stop();
                _statusCheckTimer.Dispose();
            }
        }
    }
}
