using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace FoodDeliveryApp.MVVM
{
    public partial class ModifyAddressViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        // Properties
        [ObservableProperty]
        private string userAddress; // Bound to the address Editor in XAML

        [ObservableProperty]
        private bool isSaveEnabled; // Controls whether the Save button is enabled

        [ObservableProperty]
        private bool isLoading; // Controls the loading state

        public ICommand SaveCommand { get; }

        public ModifyAddressViewModel()
        {
            _apiService = new ApiService();

            // Initialize Save command
            SaveCommand = new AsyncRelayCommand(SaveAddressAsync);

            // Load current address
            LoadCurrentAddressAsync();
        }

        private async Task LoadCurrentAddressAsync()
        {
            try
            {
                IsLoading = true;

                // Get UserId from preferences
                var userId = Preferences.Get("UserId", 0);

                if (userId==0)
                {
                    throw new Exception("User ID not found.");
                }

                // Fetch the address from the API
                UserAddress = await _apiService.GetUserAddressAsync(userId.ToString());

                IsSaveEnabled = !string.IsNullOrWhiteSpace(UserAddress);
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

        private async Task SaveAddressAsync()
        {
            if (string.IsNullOrWhiteSpace(UserAddress))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Address cannot be empty.", "OK");
                return;
            }

            try
            {
                IsSaveEnabled = false;

                // Get UserId from preferences
                var userId = Preferences.Get("UserId", 0);

                if (userId==0)
                {
                    throw new Exception("User ID not found.");
                }

                // Call API to update the address
                var isUpdated = await _apiService.UpdateUserAddressAsync(userId.ToString(), UserAddress);

                if (isUpdated)
                {
                    // Show success message
                    await Application.Current.MainPage.DisplayAlert("Success", "Address updated successfully.", "OK");

                    // Navigate back to the previous page
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    throw new Exception("Failed to update address. Please try again.");
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
