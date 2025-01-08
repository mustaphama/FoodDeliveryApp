using System.Diagnostics;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using FoodDeliveryApp.Models;


namespace FoodDeliveryApp.MVVM;

public partial class ProductDetailsViewModel : ObservableObject, IQueryAttributable
{
    private readonly ApiService _apiService;
    private readonly CartService _cartService;

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

    private const int MaxQuantity = 99; // Optional maximum limit

    public bool CanDecreaseQuantity => Quantity > 1;
    public bool CanIncreaseQuantity => Quantity < MaxQuantity;

    public ICommand IncreaseQuantityCommand { get; }
    public ICommand DecreaseQuantityCommand { get; }
    public ICommand AddToCartCommand { get; }
    public ICommand NavigateToCartCommand { get; }

    private Product _selectedProduct;
    public Product SelectedProduct
    {
        get => _selectedProduct;
        set => SetProperty(ref _selectedProduct, value);
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("SelectedProductID", out var productId) && productId is string id)
        {
            try
            {
                SelectedProduct = await _apiService.GetProductById(id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to load product details: {ex.Message}");
            }
        }
    }
    // Animation logic

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
}
