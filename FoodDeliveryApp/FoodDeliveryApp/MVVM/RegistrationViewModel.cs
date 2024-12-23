using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
namespace FoodDeliveryApp.MVVM;

public partial class RegistrationViewModel : ObservableObject
{
    private readonly ApiService _apiService;

    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private string email;

    [ObservableProperty]
    private string password;

    [ObservableProperty]
    private string phoneNumber;

    [ObservableProperty]
    private string address;

    [ObservableProperty]
    private string profilePictureUrl;

    [ObservableProperty]
    private string selectedRegionCode;

    public IAsyncRelayCommand RegisterCommand { get; }
    // Observable collection for region codes
    public ObservableCollection<string> RegionCodes { get; }

    public RegistrationViewModel()
    {
        RegionCodes = new ObservableCollection<string> { "+33", "+1", "+44" };
        _apiService = new ApiService();
        RegisterCommand = new AsyncRelayCommand(RegisterUserAsync);
    }

    private async Task RegisterUserAsync()
    {
        bool isRegistered = await _apiService.RegisterUser(Name, Email, Password, selectedRegionCode+PhoneNumber, Address, "/profile_picture.jpg");

        if (isRegistered)
        {
            // Redirect to Login Page
            await Shell.Current.GoToAsync("//LoginPage");
        }
        else
        {
            await Shell.Current.DisplaySnackbar("Registration Failed");
        }
    }
}
