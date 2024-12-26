using FoodDeliveryApp.Pages;
using System.Reflection;
namespace FoodDeliveryApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (IsUserLoggedIn())
            {
                // If logged in, navigate to the HomePage
                MainPage = new AppShell();
                Shell.Current.GoToAsync("//HomePage"); // Assuming HomePage is part of your AppShell routes
            }
            else
            {
                // If not logged in, navigate to the LoginPage (MainPage in this case)
                MainPage = new AppShell(); // AppShell handles navigation to LoginPage or RegisterPage
            }
            RegisterRoutes();


        }
        private void RegisterRoutes()
        {
            Routing.RegisterRoute("ProductDetailsPage", typeof(ProductDetailsPage));
            Routing.RegisterRoute("ModifyAddressPage", typeof(ModifyAddressPage));
            Routing.RegisterRoute("ModifyPhonePage", typeof(ModifyPhonePage));
            Routing.RegisterRoute("ProfileDetailsPage", typeof(ProfileDetailsPage));
            Routing.RegisterRoute("ModifyNamePage", typeof(ModifyNamePage));
            Routing.RegisterRoute("ModifyEmailPage", typeof(ModifyEmailPage));
            Routing.RegisterRoute("ModifyPasswordPage", typeof(ModifyPasswordPage));
        }
        private bool IsUserLoggedIn()
        {
            // Check if the user is logged in
            bool isLoggedIn = Preferences.Get("IsLoggedIn", false);
            int userId = Preferences.Get("UserId", 0); // Default to 0 if not found
            return isLoggedIn && userId > 0;
        }

    }
}
