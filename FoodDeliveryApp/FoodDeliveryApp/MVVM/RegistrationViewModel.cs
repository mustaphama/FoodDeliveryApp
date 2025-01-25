using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodDeliveryApp.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
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
    private string city;

    [ObservableProperty]
    private string country;

    [ObservableProperty]
    private string confirmPassword;

    [ObservableProperty]
    private RegionCode selectedRegionCode;

    [ObservableProperty]
    private string message;  // Property to bind to the Label in XAML

    [ObservableProperty]
    private string _messageColor = "#FF0000"; // Default color is red
    [ObservableProperty]
    private bool _messageVisibility;

    public IAsyncRelayCommand RegisterCommand { get; }
    // Observable collection for region codes
    public ObservableCollection<RegionCode> RegionCodes { get; }

    public RegistrationViewModel()
    {
        RegionCodes = new ObservableCollection<RegionCode>
        {
            new RegionCode { Code = "+1", Emoji = "🇺🇸" },
            new RegionCode { Code = "+7", Emoji = "🇷🇺" },
            new RegionCode { Code = "+20", Emoji = "🇪🇬" },
            new RegionCode { Code = "+27", Emoji = "🇿🇦" },
            new RegionCode { Code = "+30", Emoji = "🇬🇷" },
            new RegionCode { Code = "+31", Emoji = "🇳🇱" },
            new RegionCode { Code = "+32", Emoji = "🇧🇪" },
            new RegionCode { Code = "+33", Emoji = "🇫🇷" },
            new RegionCode { Code = "+34", Emoji = "🇪🇸" },
            new RegionCode { Code = "+39", Emoji = "🇮🇹" },
            new RegionCode { Code = "+40", Emoji = "🇷🇴" },
            new RegionCode { Code = "+44", Emoji = "🇬🇧" },
            new RegionCode { Code = "+49", Emoji = "🇩🇪" },
            new RegionCode { Code = "+52", Emoji = "🇲🇽" },
            new RegionCode { Code = "+55", Emoji = "🇧🇷" },
            new RegionCode { Code = "+61", Emoji = "🇦🇺" },
            new RegionCode { Code = "+62", Emoji = "🇮🇩" },
            new RegionCode { Code = "+81", Emoji = "🇯🇵" },
            new RegionCode { Code = "+82", Emoji = "🇰🇷" },
            new RegionCode { Code = "+86", Emoji = "🇨🇳" },
            new RegionCode { Code = "+91", Emoji = "🇮🇳" },
            new RegionCode { Code = "+92", Emoji = "🇵🇰" },
            new RegionCode { Code = "+93", Emoji = "🇦🇫" },
            new RegionCode { Code = "+94", Emoji = "🇱🇰" },
            new RegionCode { Code = "+98", Emoji = "🇮🇷" },
            new RegionCode { Code = "+212", Emoji = "🇲🇦" },
            new RegionCode { Code = "+213", Emoji = "🇩🇿" },
            new RegionCode { Code = "+216", Emoji = "🇹🇳" },
            new RegionCode { Code = "+218", Emoji = "🇱🇾" },
            new RegionCode { Code = "+220", Emoji = "🇬🇲" },
            new RegionCode { Code = "+234", Emoji = "🇳🇬" },
            new RegionCode { Code = "+250", Emoji = "🇷🇼" },
            new RegionCode { Code = "+251", Emoji = "🇪🇹" },
            new RegionCode { Code = "+254", Emoji = "🇰🇪" },
            new RegionCode { Code = "+255", Emoji = "🇹🇿" },
            new RegionCode { Code = "+256", Emoji = "🇺🇬" },
            new RegionCode { Code = "+260", Emoji = "🇿🇲" },
            new RegionCode { Code = "+263", Emoji = "🇿🇼" },
            new RegionCode { Code = "+351", Emoji = "🇵🇹" },
            new RegionCode { Code = "+352", Emoji = "🇱🇺" },
            new RegionCode { Code = "+353", Emoji = "🇮🇪" },
            new RegionCode { Code = "+354", Emoji = "🇮🇸" },
            new RegionCode { Code = "+355", Emoji = "🇦🇱" },
            new RegionCode { Code = "+356", Emoji = "🇲🇹" },
            new RegionCode { Code = "+358", Emoji = "🇫🇮" },
            new RegionCode { Code = "+359", Emoji = "🇧🇬" },
            new RegionCode { Code = "+370", Emoji = "🇱🇹" },
            new RegionCode { Code = "+371", Emoji = "🇱🇻" },
            new RegionCode { Code = "+372", Emoji = "🇪🇪" }
        };
        _apiService = new ApiService();
        RegisterCommand = new AsyncRelayCommand(RegisterUserAsync);
    }
    private async Task RegisterUserAsync()
    {
        if (string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword) || string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(PhoneNumber) || string.IsNullOrEmpty(Address) || string.IsNullOrEmpty(City) || string.IsNullOrEmpty(Country))
        {
            MessageVisibility = true;
            Message = "All fields are required!";
            MessageColor = "#FF0000";
        }
        else if (Password.Length < 8)
        {
            MessageVisibility = true;
            Message = "Password must be at least 8 characters long.";
            MessageColor = "#FF0000";
        }
        else
        {
            var passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$"); // Example: Min 1 lowercase, 1 uppercase, 1 digit
                                                                                     // Optionally add regex for complexity requirements
            if (!passwordRegex.IsMatch(Password))
            {
                MessageVisibility = true;
                Message = "Password must contain uppercase, lowercase, and a number.";
                MessageColor = "#FF0000";
            }
            else if (Password != ConfirmPassword)
            {
                MessageVisibility = true;
                Message = "Password doesnt match!";
                MessageColor = "#FF0000";
            }
            else
            {
                try
                {
                    bool isRegistered = await _apiService.RegisterUser(Name, Email, Password, SelectedRegionCode.Code + PhoneNumber, Address, City, Country);

                    if (isRegistered)
                    {
                        MessageVisibility = true;
                        Message = "Registered";
                        MessageColor = "#0000FF";
                        // Redirect to Login Page
                        await Shell.Current.GoToAsync("LoginPage");
                    }
                    else
                    {
                        await Shell.Current.DisplaySnackbar("Registration Failed");
                    }
                }
                catch (Exception ex)
                {
                    MessageVisibility = true;
                    Message = $"Server Error!";
                    Debug.WriteLine($"Error: {ex.Message}");
                    MessageColor = "#FF0000";
                }
            }
        }
    } 
    [RelayCommand]
    private async Task NavigateBack()
    {
        await Shell.Current.GoToAsync("..",true);
    }
}
