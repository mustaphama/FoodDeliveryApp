using FoodDeliveryApp.MVVM;
namespace FoodDeliveryApp.Pages;

public partial class CartPage : ContentPage
{
    public CartPage(CartViewModel cartViewModel)
    {
        InitializeComponent();
        BindingContext = cartViewModel; // Setting the BindingContext to CartViewModel
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is CartViewModel vm)
        {
            // Only execute the LoadCartCommand if the BindingContext is a CartViewModel
            vm.LoadCartCommand.Execute(null);
        }
    }
}
