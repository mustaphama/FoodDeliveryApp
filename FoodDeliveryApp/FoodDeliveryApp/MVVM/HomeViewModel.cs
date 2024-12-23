using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.MVVM
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        // Properties bound to XAML
        [ObservableProperty]
        private string userAddress; // For displaying the user's address in the top bar.
        [ObservableProperty]
        private string categoryIcon;

        [ObservableProperty]
        private bool isLoading; // To manage the loading state.

        public ObservableCollection<MostOrderedCategory> Categories { get; }
        public ObservableCollection<HotProduct> HotProducts { get; }

        // Commands
        public IAsyncRelayCommand SearchCommand { get; }
        public ICommand NavigateToDetailsCommand { get; }

        public HomeViewModel()
        {
            _apiService = new ApiService();

            // Initialize collections
            Categories = new ObservableCollection<MostOrderedCategory>();
            HotProducts = new ObservableCollection<HotProduct>();

            // Initialize commands
            SearchCommand = new AsyncRelayCommand(PerformSearchAsync);

            // Mock address for demonstration
            UserAddress = "123 Main St, Cityville";

            // Automatically load data
            LoadDataCommand = new AsyncRelayCommand(LoadDataAsync);
            NavigateToDetailsCommand = new AsyncRelayCommand<HotProduct>(NavigateToDetailsAsync);
        }

        public IAsyncRelayCommand LoadDataCommand { get; }

        private async Task LoadDataAsync()
        {
            IsLoading = true;

            try
            {
                // Fetch most ordered categories
                var categories = await _apiService.GetMostOrderedCategoriesAsync();
                Categories.Clear();
                foreach (var category in categories)
                {
                    Categories.Add(category);
                }

                // Fetch most ordered products
                var products = await _apiService.GetMostOrderedProductsAsync();
                HotProducts.Clear();
                foreach (var product in products)
                {
                    HotProducts.Add(product);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading data: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task PerformSearchAsync()
        {
            // Implement search functionality here
            Debug.WriteLine("Search command executed.");
            await Task.CompletedTask;
        }
        private async Task NavigateToDetailsAsync(HotProduct product)
        {
            try { 
            if (product != null)
            {
                var navigationParameter = new Dictionary<string, object>
        {
            { "SelectedProductID", product.foodItemId.ToString() }
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
    }
}
