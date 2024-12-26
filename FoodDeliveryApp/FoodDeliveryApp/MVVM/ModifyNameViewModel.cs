using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.Windows.Input;

namespace FoodDeliveryApp.MVVM;

public partial class ModifyNameViewModel : ObservableObject
{
    private readonly ApiService _apiService;


    // New email that the user wants to change to
    [ObservableProperty]
    private string newName;

    // Password for verification
    [ObservableProperty]
    private string password;

    [ObservableProperty]
    private string _statusMessage;

    [ObservableProperty]
    private Color _statusColor;

    [ObservableProperty]
    private bool _statusVisibility;

    public ModifyNameViewModel()
    {
        _apiService = new ApiService();
        StatusVisibility = false;
    }

    // Command to submit the email change
    public ICommand SubmitCommand => new AsyncRelayCommand(SubmitNameChange);


    // Submit the email change along with the password for verification
    private async Task SubmitNameChange()
    {
        try
        {
            StatusVisibility = true;
            var userId = Preferences.Get("UserId", 0);

            // Make sure the name and password are provided
            if (userId == 0)
            {
                await Shell.Current.GoToAsync("//LoginPage"); // Navigate back upon failure
                return;
            }
            else if (string.IsNullOrEmpty(NewName) || string.IsNullOrEmpty(Password))
            {
                StatusMessage = "All fields are required!";
                StatusColor = Color.FromRgb(255, 0, 0); // Error message in red  
            }
            else
            {
                var response = await _apiService.ModifyNameAsync(userId, NewName, Password);

                // Check the API response
                if (response.IsSuccess)
                {
                    StatusMessage = "Your name was changed successfully!";
                    StatusColor = Color.FromRgb(0, 255, 0); // Success message in green        
                    await Shell.Current.GoToAsync(".."); // Navigate back upon success
                }
                else
                {
                    // Log the error to diagnose why it's failing
                    StatusMessage = $"Server Error: {response.ErrorMessage}";
                    StatusColor = Color.FromRgb(255, 0, 0); // Error message in red  
                }
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions during the name update
            StatusMessage = $"Exception: {ex.Message}";
            StatusColor = Color.FromRgb(255, 0, 0); // Error message in red
            Console.WriteLine($"Error updating your name: {ex.Message}");
        }
    }

}