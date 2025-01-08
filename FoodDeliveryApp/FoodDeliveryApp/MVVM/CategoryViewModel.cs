using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodDeliveryApp.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FoodDeliveryApp.MVVM
{
    [QueryProperty(nameof(CategoryId), "categoryId")]
    public partial class CategoryViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        public CategoryViewModel()
        {
            _apiService = new ApiService();
            FoodItems = new ObservableCollection<FoodItemDto>();
            NavigateToDetailsCommand = new AsyncRelayCommand<FoodItemDto>(NavigateToDetailsAsync);
        }
        public ICommand NavigateToDetailsCommand { get; }
        // The category ID passed from navigation
        private int _categoryId;
        public int CategoryId
        {
            get => _categoryId;
            set
            {
                SetProperty(ref _categoryId, value);
                if (_categoryId > 0)
                {
                    LoadCategoryDataAsync(_categoryId);
                }
            }
        }

        // Observable property for the category's most-ordered data
        [ObservableProperty]
        private MostOrderedCategory _categoryData;

        // Observable collection of food items
        [ObservableProperty]
        private ObservableCollection<FoodItemDto> _foodItems;

        [ObservableProperty]
        private string _categoryName;

        // Method to load category data and food items asynchronously
        private async void LoadCategoryDataAsync(int categoryId)
        {
            try
            {
                Debug.WriteLine($"Loading data for category ID: {categoryId}");
                // Fetch the category name
                var category = await _apiService.GetCategoryByIdAsync(categoryId);
                CategoryName = category?.CategoryName ?? "Unknown Category";

                // Fetch food items by category
                var items = await _apiService.GetFoodItemsByCategoryAsync(categoryId);

                // Clear and populate the ObservableCollection
                FoodItems.Clear();
                foreach (var item in items)
                {
                    FoodItems.Add(item);
                }

                Debug.WriteLine($"Loaded {FoodItems.Count} food items for category ID: {categoryId}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading category data: {ex.Message}");
            }
        }
        private async Task NavigateToDetailsAsync(FoodItemDto product)
        {
            Debug.WriteLine("Clickable");
            try
            {
                if (product != null)
                {
                    var navigationParameter = new Dictionary<string, object>
        {
            { "SelectedProductID", product.FoodItemId.ToString() }
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
        [RelayCommand]
        private async Task NavigateBack()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
