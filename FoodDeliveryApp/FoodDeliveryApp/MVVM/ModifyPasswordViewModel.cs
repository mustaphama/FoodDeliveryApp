using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace FoodDeliveryApp.MVVM
{
    public partial class ModifyPasswordViewModel : ObservableObject
    {
        private readonly ApiService _apiservice;

        [ObservableProperty]
        private string oldPassword;

        [ObservableProperty]
        private string newPassword;

        [ObservableProperty]
        private string confirmNewPassword;

        [ObservableProperty]
        private string _statusMessage;

        [ObservableProperty]
        private Color _statusColor;

        [ObservableProperty]
        private bool _statusVisibility;

        public ICommand ChangePasswordCommand { get; }

        public ModifyPasswordViewModel()
        {
            StatusVisibility = false;
            _apiservice = new ApiService(); // Ensure you configure the base address elsewhere
            ChangePasswordCommand = new AsyncRelayCommand(ChangePasswordAsync);
        }

        private async Task ChangePasswordAsync()
        {
            try
            {
                var userId = Preferences.Get("UserId", 0); // Get UserId from preferences
                if (userId == 0)
                {
                    // Navigate back upon success
                    await Shell.Current.GoToAsync("LoginPage");
                    // Navigate back upon success
                }
                // Validate input
                else if (string.IsNullOrWhiteSpace(OldPassword) ||
                    string.IsNullOrWhiteSpace(NewPassword) ||
                    string.IsNullOrWhiteSpace(ConfirmNewPassword))
                {
                    StatusVisibility = true;
                    StatusMessage = "All fields are required!";
                    StatusColor = Color.FromRgb(255, 0, 0);
                }
                else if (NewPassword.Length < 8)
                {
                    StatusVisibility = true;
                    StatusMessage = "Password must be at least 8 characters long.";
                    StatusColor = Color.FromRgb(255, 0, 0);
                }
                else
                {
                    var passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$"); // Example: Min 1 lowercase, 1 uppercase, 1 digit
                                                                     // Optionally add regex for complexity requirements
                    if (!passwordRegex.IsMatch(NewPassword))
                    {
                        StatusVisibility = true;
                        StatusMessage = "Password must contain uppercase, lowercase, and a number.";
                        StatusColor = Color.FromRgb(255, 0, 0);
                    }
                    else if (NewPassword != ConfirmNewPassword)
                    {
                        StatusVisibility = true;
                        StatusMessage = "Password doesnt match!";
                        StatusColor = Color.FromRgb(255, 0, 0);
                    }
                    else
                    {
                        var result = await _apiservice.ModifyPasswordAsync(userId, OldPassword, NewPassword);
                        StatusVisibility = true;
                        OnPropertyChanged(nameof(StatusVisibility));
                        if (result.IsSuccess)
                        {
                            StatusMessage = "Your password was changed successfully!";
                            StatusColor = Color.FromRgb(0, 255, 0); // Success message in green        
                            await Shell.Current.GoToAsync(".."); // Navigate back upon success
                        }
                        else
                        {
                            StatusMessage = "Server Error: " + result.ErrorMessage;
                            StatusColor = Color.FromRgb(255, 0, 0);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                StatusVisibility = true;
                StatusMessage = ex.Message;
                StatusColor = Color.FromRgb(255, 0, 0);
            }
        }
    }
}
