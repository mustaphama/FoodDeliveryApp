using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FoodDeliveryApp.Models;
using System.Diagnostics;
using System.Windows.Input;
namespace FoodDeliveryApp.MVVM
{
    public partial class SearchViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        public SearchViewModel()
        {
            _apiService = new ApiService();
            SearchResults = new ObservableCollection<FoodItemDto>();
            NavigateToDetailsCommand = new AsyncRelayCommand<FoodItemDto>(NavigateToDetailsAsync);
        }
        public ICommand NavigateToDetailsCommand { get; }


        [ObservableProperty]
        private string searchQuery;

        [ObservableProperty]
        private ObservableCollection<FoodItemDto> searchResults;

        [RelayCommand]
        private async Task PerformSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
                return;

            var results = await _apiService.SearchFoodItemsAsync(SearchQuery);
            SearchResults.Clear();

            foreach (var item in results)
                SearchResults.Add(item);
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
    [RelayCommand]
    private async Task NavigateBack()
    {
        await Shell.Current.GoToAsync("..");
    }
    }
}