using System.Diagnostics;
using FoodDeliveryApp.MVVM;
namespace FoodDeliveryApp.Pages;

public partial class ProductDetailsPage : ContentPage
{
    private Button _addToCartButton;
    private Button _checkCartButton;
    public ProductDetailsPage(ProductDetailsViewModel productdetailsviewmodel)
    {
        InitializeComponent();
        BindingContext = productdetailsviewmodel;
        Shell.Current.FindByName<TabBar>("MainTabBar").IsVisible = true;
        // Initialize button references
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ProductDetailsViewModel viewModel)
        {
            viewModel.ResetState();
        }

        // Initialize button references (using the generic version)
        _addToCartButton = this.FindByName<Button>("AddToCartButton");
        _checkCartButton = this.FindByName<Button>("CheckCartButton");

        // Ensure buttons are found
        if (_addToCartButton != null && _checkCartButton != null)
        {
            // Hide the check cart button initially
            _checkCartButton.IsVisible = false;
            _addToCartButton.IsVisible = true;
            // Reset the "Add to Cart" button's appearance
            _addToCartButton.Scale = 1;
        }
    }
    // Method to animate the button morph
    private async Task MorphButtonAnimation()
    {
        // Step 1: Start animation by scaling down the "Add to Cart" button
        await _addToCartButton.ScaleTo(0.5333, 250, Easing.Linear);
        _addToCartButton.CornerRadius = 50;
        // Step 3: Fade out the "Add to Cart" button
        await _addToCartButton.FadeTo(0, 250);

        _addToCartButton.IsVisible = false;

        // Step 4: Make the "Check Cart" button visible
        _checkCartButton.IsVisible = true;

        // Step 5: Fade in the "Check Cart" button
        await _checkCartButton.FadeTo(1, 250);

        // Step 6: Scale the "Check Cart" button back to normal size
        await _checkCartButton.ScaleTo(1, 250, Easing.Linear);
        await _addToCartButton.FadeTo(1, 250); // Make sure it fades in when it's shown
        _addToCartButton.CornerRadius = 10;

        // Step 7: Optionally hide the "Add to Cart" button once the animation is done
    }

    // Example method that can be called when the "Add to Cart" button is clicked
    private async void OnAddToCartClicked(object sender, EventArgs e)
    {
        // Perform the morph animation
        await MorphButtonAnimation();
    }
    protected override bool OnBackButtonPressed()
    {
        // Navigate back to the previous page
        Shell.Current.GoToAsync("..");
        Debug.WriteLine("Back button pressed");
        Debug.WriteLine($"Current route: {Shell.Current.CurrentState.Location}");
        return true; // Prevent default back button behavior
    }


}