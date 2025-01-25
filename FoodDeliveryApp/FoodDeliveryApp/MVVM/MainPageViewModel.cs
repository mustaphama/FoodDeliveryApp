using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodDeliveryApp.Pages;
using System.Threading.Tasks;
namespace FoodDeliveryApp.MVVM;

public partial class MainPageViewModel : ObservableObject
{
    public IAsyncRelayCommand NavigateToLoginCommand { get; }
    public IAsyncRelayCommand NavigateToRegisterCommand { get; }

    public MainPageViewModel()
    {
        NavigateToLoginCommand = new AsyncRelayCommand(NavigateToLogin);
        NavigateToRegisterCommand = new AsyncRelayCommand(NavigateToRegister);
    }

    private async Task NavigateToLogin()
    {
        await Shell.Current.GoToAsync("LoginPage");
    }

    private async Task NavigateToRegister()
    {
        await Shell.Current.GoToAsync("RegisterPage");
    }
}
