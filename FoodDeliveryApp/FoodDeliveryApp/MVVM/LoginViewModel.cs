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

    [ObservableProperty]
    private string _messageColor = "#FF0000"; // Default color is red
    [ObservableProperty]
    private bool _messageVisibility;

    public IAsyncRelayCommand LoginCommand { get; }

    public LoginViewModel()
    {
        _apiService = new ApiService();
        LoginCommand = new AsyncRelayCommand(LoginUserAsync);
        MessageVisibility = false;
    }

    // Async method to call the API service for logging in
    private async Task LoginUserAsync()
    {
        var response = await _apiService.LoginUser(Email, Password);

        if (response.IsSuccess)
        {
            // Store the Id_Users in Preferences
            if (response.Id_Users > 0)
            {
                // Set login status
                Preferences.Set("IsLoggedIn", true);
                Preferences.Set("UserId", response.Id_Users);
            }
            MessageVisibility = true;
            Message = "Login successful!";
            MessageColor = "#00FF00"; // Change color to green

            // Show the MainTabBar and navigate to HomePage
            Shell.Current.FindByName<TabBar>("MainTabBar").IsVisible = true;
            await Shell.Current.GoToAsync("//HomePage");
        }
        else
        {
            MessageVisibility = true;
            Message = response.ErrorMessage ?? "Login failed.";
            MessageColor = "#FF0000"; // Change color to red
        }
    }
    [RelayCommand]
    private async Task NavigateBack()
    {
        await Shell.Current.GoToAsync("..", true);
    }


}
