using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

namespace FoodDeliveryApp.MVVM
{
    public partial class ModifyEmailViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        // The current email of the user (retrieved from the API)
        [ObservableProperty]
        private string currentEmail;

        // New email that the user wants to change to
        [ObservableProperty]
        private string newEmail;

        // Password for verification
        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string _statusMessage;

        [ObservableProperty]
        private Color _statusColor;

        [ObservableProperty]
        private bool _statusVisibility;

        public ModifyEmailViewModel()
        {
            _apiService = new ApiService();
            StatusVisibility = false;
            GetCurrentEmail();
        }

        // Command to fetch the current email and display it
        public ICommand GetCurrentEmailCommand => new AsyncRelayCommand(GetCurrentEmail);

        // Command to submit the email change
        public ICommand SubmitCommand => new AsyncRelayCommand(SubmitEmailChange);

        private async Task GetCurrentEmail()
        {
            try
            {
                // Get the Id_Users from preferences (assuming it was saved previously)
                var userId = Preferences.Get("UserId", 0);
                if (userId == 0)
                {
                    StatusMessage = "User ID not found. Please log in again.";
                    StatusColor = Color.FromRgb(255, 0, 0); // Red color for error
                    StatusVisibility = true;
                    return;
                }

                // Call the API to get the current email of the user
                var currentEmail = await _apiService.GetUserEmailAsync(userId);
                if (string.IsNullOrEmpty(currentEmail))
                {
                    StatusMessage = "Failed to fetch current email.";
                    StatusColor = Color.FromRgb(255, 0, 0); // Red color for error
                    StatusVisibility = true;
                }
                else
                {
                    CurrentEmail = currentEmail; // This will automatically update the binding
                    Debug.WriteLine($"Current email fetched: {CurrentEmail}");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions (e.g., failed to fetch email)
                StatusMessage = $"Error fetching current email: {ex.Message}";
                StatusColor = Color.FromRgb(255, 0, 0); // Red color for error
                StatusVisibility = true;
                Console.WriteLine($"Error fetching current email: {ex.Message}");
            }
        }


        // Submit the email change along with the password for verification
        private async Task SubmitEmailChange()
        {
            try
            {
                StatusVisibility = true;
                var userId = Preferences.Get("UserId", 0);

                // Make sure the email is different and that the password is provided
                if (userId == 0)
                {
                    await Shell.Current.GoToAsync("//LoginPage"); // Navigate back upon success
                    return;
                } else if (string.IsNullOrEmpty(NewEmail) || string.IsNullOrEmpty(Password))
                {
                    StatusMessage = "All fields are required!";
                    StatusColor = Color.FromRgb(255, 0, 0); // Success message in green  
                }
                else {

                var response = await _apiService.ModifyEmailAsync(userId, NewEmail, Password);

                if (response.IsSuccess)
                {
                    StatusMessage = "Your email was changed successfully!";
                    StatusColor = Color.FromRgb(0, 255, 0); // Success message in green        
                    await Shell.Current.GoToAsync(".."); // Navigate back upon success
                }
                else
                {
                    StatusMessage = "Server Error, please try again later.";
                    StatusColor = Color.FromRgb(255, 0, 0); // Success message in green  
                }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions during email update
                Console.WriteLine($"Error updating email: {ex.Message}");
            }
        }
    }
}
