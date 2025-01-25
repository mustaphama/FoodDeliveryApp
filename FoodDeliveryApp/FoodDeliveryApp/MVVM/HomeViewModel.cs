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
        public ObservableCollection<FoodItemDto> HotProducts { get; }
        public ObservableCollection<FoodItemDto> HighRatedProducts { get; }
        public ObservableCollection<FoodItemDto> NewestProducts { get; }
        public ObservableCollection<FoodItemDto> CheapestProducts { get; }


        public ICommand NavigateToDetailsCommand { get; }
        public ICommand NavigateToCategoryCommand { get; }

        public ICommand NavigateToSearchCommand { get; }

        public HomeViewModel()
        {
            _apiService = new ApiService();

            // Initialize collections
            Categories = new ObservableCollection<MostOrderedCategory>();
            HotProducts = new ObservableCollection<FoodItemDto>();
            HighRatedProducts = new ObservableCollection<FoodItemDto>();
            NewestProducts = new ObservableCollection<FoodItemDto>();
            CheapestProducts = new ObservableCollection<FoodItemDto>();

            // Load current address
            LoadCurrentAddressAsync();

            // Automatically load data
            LoadDataCommand = new AsyncRelayCommand(LoadDataAsync);
            NavigateToDetailsCommand = new AsyncRelayCommand<FoodItemDto>(NavigateToDetailsAsync);
            NavigateToCategoryCommand = new Command<MostOrderedCategory>(NavigateToCategory);
            NavigateToSearchCommand = new AsyncRelayCommand(NavigateToSearch);
        }

        public IAsyncRelayCommand LoadDataCommand { get; }

        private async Task LoadDataAsync()
        {
            IsLoading = true;
            LoadCurrentAddressAsync();
            try
            {
                // Fetch most ordered categories
                var categories = await _apiService.GetMostOrderedCategoriesAsync();
                Categories.Clear();
                foreach (var category in categories)
                {
                    Categories.Add(category);
                }


                var userId = Preferences.Get("UserId", 0);

                if (userId == 0)
                {
                    await Shell.Current.GoToAsync("MainPage");
                }

                // Fetch most ordered products
                var hotproducts = await _apiService.GetMostOrderedProductsAsync(userId);
                HotProducts.Clear();
                foreach (var product in hotproducts)
                {
                    HotProducts.Add(product);
                }
                // Fetch highest rated products
                var highratedproducts = await _apiService.GetHighestRatedProductsAsync(userId);
                HighRatedProducts.Clear();
                foreach (var product in highratedproducts)
                {
                    HighRatedProducts.Add(product);
                }
                // Fetch the recently added products
                var newproducts = await _apiService.GetNewestFoodItemsAsync(userId);
                NewestProducts.Clear();
                foreach (var product in newproducts)
                {
                    NewestProducts.Add(product);
                }
                // Fetch cheapest products
                var cheapproducts = await _apiService.GetCheapestFoodItemsAsync(userId);
                CheapestProducts.Clear();
                foreach (var product in cheapproducts)
                {
                    CheapestProducts.Add(product);
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

                // Get Id_Users from preferences
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
        private async Task NavigateToDetailsAsync(FoodItemDto product)
        {
            try {
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
        private async void NavigateToCategory(MostOrderedCategory category)
        {
            if (category != null)
            {
                await Shell.Current.GoToAsync($"CategoryPage?categoryId={category.Id_Categories}");
            }
        }
        private async Task NavigateToSearch()
        {
                await Shell.Current.GoToAsync("SearchPage");
        }
    }
}
