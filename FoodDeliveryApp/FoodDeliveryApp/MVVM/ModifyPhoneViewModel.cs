using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace FoodDeliveryApp.MVVM
{
    public partial class ModifyPhoneViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        // Properties
        [ObservableProperty]
        private string userPhone; // Bound to the Phone Editor in XAML

        [ObservableProperty]
        private bool isSaveEnabled; // Controls whether the Save button is enabled

        [ObservableProperty]
        private bool isLoading; // Controls the loading state

        public ICommand SaveCommand { get; }

        public ModifyPhoneViewModel()
        {
            _apiService = new ApiService();

            // Initialize Save command
            SaveCommand = new AsyncRelayCommand(SavePhoneAsync);

            // Load current Phone
            LoadCurrentPhoneAsync();
        }

        private async Task LoadCurrentPhoneAsync()
        {
            try
            {
                IsLoading = true;

                // Get UserId from preferences
                var userId = Preferences.Get("UserId", 0);

                if (userId == 0)
                {
                    throw new Exception("User ID not found.");
                }

                // Fetch the Phone from the API
                UserPhone = await _apiService.GetUserPhoneAsync(userId.ToString());

                IsSaveEnabled = !string.IsNullOrWhiteSpace(UserPhone);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task SavePhoneAsync()
        {
            if (string.IsNullOrWhiteSpace(UserPhone))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Phone cannot be empty.", "OK");
                return;
            }

            try
            {
                IsSaveEnabled = false;

                // Get UserId from preferences
                var userId = Preferences.Get("UserId", 0);

                if (userId == 0)
                {
                    throw new Exception("User ID not found.");
                }

                // Call API to update the Phone
                var isUpdated = await _apiService.UpdateUserPhoneAsync(userId.ToString(), UserPhone);

                if (isUpdated)
                {
                    // Show success message
                    await Application.Current.MainPage.DisplayAlert("Success", "Phone number updated successfully.", "OK");

                    // Navigate back to the previous page
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    throw new Exception("Failed to update Phone number. Please try again.");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsSaveEnabled = true;
            }
        }
    }
}
