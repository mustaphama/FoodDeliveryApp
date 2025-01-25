using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace FoodDeliveryApp.MVVM
{
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        [ObservableProperty]
        private string userName; // User's name to be displayed
        [ObservableProperty]
        private string email; // User's email
        [ObservableProperty]
        private string phoneNumber; // User's phone number

        [ObservableProperty]
        private bool isLoading; // To manage loading state

        // Commands
        public ICommand NavigateToProfileDetailsCommand { get; }
        public ICommand ModifyAddressCommand { get; }
        public ICommand ChangePhoneNumberCommand { get; }
        public ICommand LogoutCommand { get; }

        private readonly CartService _cartService;
        public ProfileViewModel(CartService cartService)
        {
            _apiService = new ApiService();
            _cartService = cartService;
            // Initialize commands
            NavigateToProfileDetailsCommand = new AsyncRelayCommand(NavigateToProfileDetailsAsync);
            ModifyAddressCommand = new AsyncRelayCommand(ModifyAddressAsync);
            ChangePhoneNumberCommand = new AsyncRelayCommand(ChangePhoneNumberAsync);
            LogoutCommand = new AsyncRelayCommand(LogoutAsync);

            // Load user data from preferences
            LoadUserProfile();
        }

        private void LoadUserProfile()
        {
            // Get Id_Users from preferences (already set when user logs in)
            var userId = Preferences.Get("UserId", 0);

            if (userId!=0)
            {
                // Call API to get user profile data
                LoadUserDataAsync(userId.ToString());
                Debug.WriteLine($"User ID: {userId}");
            }
        }

        private async Task LoadUserDataAsync(string userId)
        {
            try
            {
                // Fetch user profile details from API
                var userProfile = await _apiService.GetUserProfileAsync(userId);
                Debug.WriteLine($"User Profile: {userProfile}");

                // Bind the properties to UI
                UserName = userProfile.Name;
                Email = userProfile.Email;
                PhoneNumber = userProfile.PhoneNumber;
                OnPropertyChanged(nameof(UserName));
            }
            catch (Exception ex)
            {
                // Handle errors gracefully
                Debug.WriteLine($"Error loading user profile: {ex.Message}");
            }
        }

        private async Task NavigateToProfileDetailsAsync()
        {
            // Navigate to ProfileDetailsPage where user can change their profile details
            await Shell.Current.GoToAsync("ProfileDetailsPage");
        }

        private async Task ModifyAddressAsync()
        {
            await Shell.Current.GoToAsync("ModifyAddressPage");
        }

        private async Task ChangePhoneNumberAsync()
        {
            // Navigate to ChangePhoneNumberPage where user can change their phone number
            await Shell.Current.GoToAsync("ModifyPhonePage");
        }

        private async Task LogoutAsync()
        {
            // Show confirmation dialog before logging out
            bool confirmed = await Application.Current.MainPage.DisplayAlert(
                "Confirm Logout",
                "Are you sure you want to log out?",
                "Yes",
                "No");

            if (confirmed)
            {
                // Clear user-related preferences
                Preferences.Clear();
                _cartService.ClearCart();
                // Restart the app after logout
                await Shell.Current.GoToAsync("//MainPage");
            }
        }

        private void RestartApp()
        {
            // Restart the application to go back to the login page
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
