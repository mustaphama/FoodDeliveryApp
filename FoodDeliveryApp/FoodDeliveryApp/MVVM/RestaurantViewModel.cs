using System.Security.Cryptography.X509Certificates;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodDeliveryApp.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FoodDeliveryApp.MVVM
{
    [QueryProperty(nameof(RestaurantId), "restaurantId")]
    public partial class RestaurantViewModel : ObservableObject
    {

        private int _restaurantId;
        private readonly ApiService _apiService;
        public ICommand NavigateToDetailsCommand { get; }
        public RestaurantViewModel()
        {
            _apiService = new ApiService();
            Menus = new ObservableCollection<Menu>();
            NavigateToDetailsCommand = new AsyncRelayCommand<FoodItemDto>(NavigateToDetailsAsync);
        }
        public int RestaurantId
        {
            get => _restaurantId;
            set
            {
                SetProperty(ref _restaurantId, value);
                if (_restaurantId > 0)
                {
                    LoadRestaurantDataAsync(_restaurantId);
                }
            }
        }
        public ObservableCollection<Menu> Menus { get; }
        [ObservableProperty]
        public RestaurantDto _restaurant;
        [ObservableProperty]
        public int _idFirstItem;

        public async Task LoadRestaurantDataAsync(int restaurantId)
        {
            try
            {
                // Fetch Restaurant Details
                Restaurant = await _apiService.GetRestaurantById(restaurantId);

                // Fetch Menus for the Restaurant
                var menus = await _apiService.GetMenusByRestaurantId(restaurantId);
                Menus.Clear();

                foreach (var menu in menus)
                {
                    // Fetch Food Items for Each Menu
                    var foodItems = await _apiService.GetFoodItemsByMenuId(menu.Id);

                    Menus.Add(new Menu
                    {
                        Id = menu.Id,
                        Name = menu.Name,
                        FoodItems = new ObservableCollection<FoodItemDto>(foodItems)
                    });
                    IdFirstItem = Menus[0].FoodItems[0].Id;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"Error loading restaurant details: {ex.Message}");
            }
        }
        [RelayCommand]
        private async Task NavigateBack()
        {
            await Shell.Current.GoToAsync("..", true);
        }
        private async Task NavigateToDetailsAsync(FoodItemDto product)
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
    }
}