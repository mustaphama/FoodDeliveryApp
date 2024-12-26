using FoodDeliveryApp.MVVM;
namespace FoodDeliveryApp.Pages;

public partial class ProfilePage : ContentPage
{
	public ProfilePage(ProfileViewModel profileViewModel)
	{
		InitializeComponent();
        BindingContext = profileViewModel;
    }
}