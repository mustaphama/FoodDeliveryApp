using FoodDeliveryApp.MVVM;
namespace FoodDeliveryApp.Pages;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
	 InitializeComponent();
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is HomeViewModel viewModel)
        {
            await viewModel.LoadDataCommand.ExecuteAsync(null);
        }
    }

}