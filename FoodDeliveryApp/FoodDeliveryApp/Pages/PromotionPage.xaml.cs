using FoodDeliveryApp.MVVM;
namespace FoodDeliveryApp.Pages;

public partial class PromotionPage : ContentPage
{
	public PromotionPage()
	{
		InitializeComponent();
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is PromotionViewModel viewModel)
        {
            await viewModel.LoadPromotionCardDataAsync();
        }
    }
}