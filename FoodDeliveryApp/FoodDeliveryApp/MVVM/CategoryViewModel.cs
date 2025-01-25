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
        private const int PageSize = 5; // Number of items to load per page

        public CategoryViewModel()
        {
            _apiService = new ApiService();
            FoodItems = new ObservableCollection<FoodItemDto>();
            NavigateToDetailsCommand = new AsyncRelayCommand<FoodItemDto>(NavigateToDetailsAsync);
        }

        public ICommand NavigateToDetailsCommand { get; }
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
        [ObservableProperty]
        private MostOrderedCategory _categoryData;

        [ObservableProperty]
        private ObservableCollection<FoodItemDto> _foodItems;

        [ObservableProperty]
        private string _categoryName;

        // For paging
        private int _currentPage = 0;
        private bool _isLoadingMore = false;
        private bool _hasMoreItems = true;

        private async void LoadCategoryDataAsync(int categoryId)
        {
            try
            {
                Debug.WriteLine($"Loading data for category ID: {categoryId}");

                // Fetch the category name
                var category = await _apiService.GetCategoryByIdAsync(categoryId);
                CategoryName = category?.CategoryName ?? "Unknown Category";

                // Load the first page of items
                _currentPage = 0;
                _hasMoreItems = true;
                FoodItems.Clear();
                await LoadMoreItemsAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading category data: {ex.Message}");
            }
        }
        private bool _isThrottleActive = false;
        private async Task LoadMoreItemsAsync()
        {
            if (_isLoadingMore || !_hasMoreItems || _isThrottleActive)
                return;

            _isThrottleActive = true;

            try
            {
                _isLoadingMore = true;
                _currentPage++;

                var items = await _apiService.GetFoodItemsByCategoryAsync(CategoryId, _currentPage, PageSize);
                if (items.Items.Count() < PageSize)
                {
                    _hasMoreItems = false;
                }

                foreach (var item in items.Items)
                {
                    FoodItems.Add(item);
                }

                Debug.WriteLine($"Loaded page {_currentPage}, total items: {FoodItems.Count}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading more items: {ex.Message}");
            }
            finally
            {
                _isLoadingMore = false;
                await Task.Delay(1000); // Wait before allowing another load
                _isThrottleActive = false;
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

        [RelayCommand]
        private async Task NavigateBack()
        {
            await Shell.Current.GoToAsync("..", true);
        }

        [RelayCommand]
        private async Task LoadMore()
        {
            Debug.WriteLine("Triggered");
            // Triggered when user pulls up
            await LoadMoreItemsAsync();
        }
    }
}
