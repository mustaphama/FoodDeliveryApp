using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodDeliveryApp.Models;


namespace FoodDeliveryApp.MVVM;

public partial class ProductDetailsViewModel : ObservableObject, IQueryAttributable
{
    private readonly ApiService _apiService;
    private readonly CartService _cartService;
    public ICommand NavigateToRestaurantCommand { get; }
    public ObservableCollection<FoodItemDto> Recommendations { get; }

    public ProductDetailsViewModel(CartService cartService)
    {
        _cartService = cartService;
        _apiService = new ApiService();
        IncreaseQuantityCommand = new Command(() =>
        {
            if (Quantity < MaxQuantity)
                Quantity++;
        });

        DecreaseQuantityCommand = new Command(() =>
        {
            if (Quantity > 1)
                Quantity--;
            
        });
        AddToCartCommand = new Command(AddToCart);
        NavigateToCartCommand = new Command(NavigateToCart);
        IsSubmitEnabled = true;
        NavigateToRestaurantCommand = new Command<FoodItemDto>(NavigateToRestaurant);
        Recommendations = new ObservableCollection<FoodItemDto>();
    }
    private int _quantity = 1;
    public int Quantity
    {
        get => _quantity;
        set
        {
            SetProperty(ref _quantity, value);
            OnPropertyChanged(nameof(CanDecreaseQuantity));
            OnPropertyChanged(nameof(CanIncreaseQuantity));
        }
    }
    private int _selectedRating = 0;
    public int SelectedRating
    {
        get => _selectedRating;
        set
        {
            if (_selectedRating != value)
            {
                _selectedRating = value;
                OnPropertyChanged();
                UpdateStarIcons(); // Update UI based on rating
            }
        }
    }
    [ObservableProperty]
    public bool _isSubmitEnabled = true;
    [ObservableProperty]
    public string _message;

    public ICommand TapStar1 => new Command(() => SelectedRating = 1);
    public ICommand TapStar2 => new Command(() => SelectedRating = 2);
    public ICommand TapStar3 => new Command(() => SelectedRating = 3);
    public ICommand TapStar4 => new Command(() => SelectedRating = 4);
    public ICommand TapStar5 => new Command(() => SelectedRating = 5);

    public ICommand SubmitRatingCommand => new Command(SubmitRating);
    [ObservableProperty]
    public string _star1FontFamily = "FontAwesome_Regular";
    [ObservableProperty]
    public string _star2FontFamily = "FontAwesome_Regular";
    [ObservableProperty]
    public string _star3FontFamily = "FontAwesome_Regular";
    [ObservableProperty]
    public string _star4FontFamily = "FontAwesome_Regular";
    [ObservableProperty]
    public string _star5FontFamily = "FontAwesome_Regular";

    private const int MaxQuantity = 99; // Optional maximum limit

    public bool CanDecreaseQuantity => Quantity > 1;
    public bool CanIncreaseQuantity => Quantity < MaxQuantity;

    public ICommand IncreaseQuantityCommand { get; }
    public ICommand DecreaseQuantityCommand { get; }
    public ICommand AddToCartCommand { get; }
    public ICommand NavigateToCartCommand { get; }

    [ObservableProperty]
    private FoodItemDto _selectedProduct;

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("SelectedProductID", out var productId) && productId is string id)
        {
            try
            {
                SelectedProduct = await _apiService.GetProductById(id);
                LoadDataAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to load product details: {ex.Message}");
            }
        }
    }
    // Animation logic
    [RelayCommand]
    private async Task LoadDataAsync()
    {
        try
        {
            // Fetch most ordered products
            var products = await _apiService.GetRecommendationsAsync(SelectedProduct.Id);
            Recommendations.Clear();
            foreach (var product in products)
            {
                Recommendations.Add(product);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading product details: {ex.Message}");
        }
    }
        private void AddToCart()
    {
        if (SelectedProduct == null)
        {
            Debug.WriteLine("No product selected to add to the cart.");
            return;
        }
        if (SelectedProduct == null)
        {
            Debug.WriteLine("SelectedProduct is null.");
            return;
        }

        if (_cartService == null)
        {
            Debug.WriteLine("CartService is not initialized.");
            return;
        }

        if (Quantity <= 0)
        {
            Debug.WriteLine("Invalid quantity.");
            return;
        }

        try
        {
            _cartService.AddOrUpdateCartItem(
                foodItemId: SelectedProduct.Id,
                quantity: Quantity,
                price: SelectedProduct.Price
            );

            Debug.WriteLine($"{Quantity} x {SelectedProduct.Name} added to cart.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error adding item to cart: {ex.Message}");
        }
    }
    private async void NavigateToCart()
    {
        await Shell.Current.GoToAsync("//CartPage");
    }
    [RelayCommand]
    private async Task NavigateBack()
    {
        await Shell.Current.GoToAsync("..", true);
    }

    private void UpdateStarIcons()
    {
        // Update the UI for each star (empty or filled based on selected rating)
        Star1FontFamily = SelectedRating >= 1 ? "FontAwesome" : "FontAwesome_Regular";
        Star2FontFamily = SelectedRating >= 2 ? "FontAwesome" : "FontAwesome_Regular";
        Star3FontFamily = SelectedRating >= 3 ? "FontAwesome" : "FontAwesome_Regular";
        Star4FontFamily = SelectedRating >= 4 ? "FontAwesome" : "FontAwesome_Regular";
        Star5FontFamily = SelectedRating >= 5 ? "FontAwesome" : "FontAwesome_Regular";
    }
    public async void SubmitRating()
    {
        Debug.WriteLine("Submit Button Clicked");
        var response = await _apiService.SubmitProductRatingAsync(new RatingRequest
        {
            UserId = Preferences.Get("UserId", 0),
            FoodItemId = SelectedProduct.Id, // This should be dynamic
            Rating = SelectedRating
        });

        if (response.IsSuccess)
        {
            // On success: Hide stars, button and show success message
            IsSubmitEnabled = false;
            Message = "Thanks for your rating!";
        }
        else
        {
            // On error: Show error message
            Message = response.ErrorMessage;
            IsSubmitEnabled = false;
        }
    }
    public void ResetState()
    {
        IsSubmitEnabled = true;
        SelectedRating = 0; // Reset rating if needed
        Message = string.Empty; // Clear the message
        UpdateStarIcons(); // Reset star icons to default
    }
    private async void NavigateToRestaurant(FoodItemDto foodItem)
    {
        if (foodItem != null)
        {
            await Shell.Current.GoToAsync($"RestaurantPage?restaurantId={foodItem.Restaurant.Id}");
        }
    }

}

