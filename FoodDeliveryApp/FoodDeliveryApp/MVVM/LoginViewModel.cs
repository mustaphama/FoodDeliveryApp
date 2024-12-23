using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
namespace FoodDeliveryApp.MVVM;
public partial class LoginViewModel : ObservableObject
{
    private readonly ApiService _apiService;

    [ObservableProperty]
    private string email;

    [ObservableProperty]
    private string password;

    [ObservableProperty]
    private string message;  // Property to bind to the Label in XAML

    public IAsyncRelayCommand LoginCommand { get; }

    public LoginViewModel()
    {
        _apiService = new ApiService();
        LoginCommand = new AsyncRelayCommand(LoginUserAsync);
    }

    // Async method to call the API service for logging in
    private async Task LoginUserAsync()
    {
        var response = await _apiService.LoginUser(Email, Password);

        if (response.IsSuccess)
        {
            // Store the UserId in Preferences
            if (response.UserId > 0)
            {
                // Set login status
                Preferences.Set("IsLoggedIn", true);
                Preferences.Set("UserId", response.UserId);
            }

            Message = "Login successful!";

            // Show the MainTabBar and navigate to HomePage
            Shell.Current.FindByName<TabBar>("MainTabBar").IsVisible = true;
            await Shell.Current.GoToAsync("//HomePage");
        }
        else
        {
            Message = response.ErrorMessage ?? "Login failed.";
        }
    }


}
