using FoodDeliveryApp.MVVM;
namespace FoodDeliveryApp.Pages;

public partial class DeliveryDetailsPage : ContentPage
{
	public DeliveryDetailsPage(DeliveryDetailsViewModel deliveryDetailsViewModel) 
	{
		InitializeComponent();
        BindingContext = deliveryDetailsViewModel; // Setting the BindingContext to DeliveryDetailsViewModel
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is DeliveryDetailsViewModel vm)
        {
            // Only execute the LoadCartCommand if the BindingContext is a DeliveryDetailsViewModel
            vm.LoadCartCommand.Execute(null);
        }
    }
}