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

        public ICommand NavigateToDetailsCommand { get; }
        public ICommand NavigateToCategoryCommand { get; }

        public ICommand NavigateToSearchCommand { get; }

        public HomeViewModel()
        {
            _apiService = new ApiService();

            // Initialize collections
            Categories = new ObservableCollection<MostOrderedCategory>();
            HotProducts = new ObservableCollection<HotProduct>();

            // Load current address
            LoadCurrentAddressAsync();

            // Automatically load data
            LoadDataCommand = new AsyncRelayCommand(LoadDataAsync);
            NavigateToDetailsCommand = new AsyncRelayCommand<HotProduct>(NavigateToDetailsAsync);
            NavigateToCategoryCommand = new Command<MostOrderedCategory>(NavigateToCategory);
            NavigateToSearchCommand = new AsyncRelayCommand(NavigateToSearch);
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
        private async Task LoadCurrentAddressAsync()
        {
            try
            {
                IsLoading = true;

                // Get UserId from preferences
                var userId = Preferences.Get("UserId", 0);

                if (userId == 0)
                {
                    throw new Exception("User ID not found.");
                }

                // Fetch the address from the API
                UserAddress = await _apiService.GetUserAddressAsync(userId.ToString());
                OnPropertyChanged(UserAddress);

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
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
        private async void NavigateToCategory(MostOrderedCategory category)
        {
            if (category != null)
            {
                await Shell.Current.GoToAsync($"CategoryPage?categoryId={category.CategoryId}");
            }
        }
        private async Task NavigateToSearch()
        {
                await Shell.Current.GoToAsync("SearchPage");
        }
    }
}
