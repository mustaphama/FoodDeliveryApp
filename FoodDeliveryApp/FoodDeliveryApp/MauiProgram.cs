using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using FoodDeliveryApp.MVVM;
using FoodDeliveryApp.Pages;

namespace FoodDeliveryApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Font Awesome 6 Free-Solid-900.otf", "FontAwesome");

                });
            // Register CartService as a Singleton
            builder.Services.AddSingleton<CartService>();

            // Register ViewModels with DI
            builder.Services.AddSingleton<ProductDetailsViewModel>();
            builder.Services.AddSingleton<CartViewModel>();
            builder.Services.AddSingleton<ProfileViewModel>();
            builder.Services.AddSingleton<DeliveryDetailsViewModel>();

            // Register Pages with DI
            builder.Services.AddSingleton<CartPage>();
            builder.Services.AddSingleton<ProductDetailsPage>();
            builder.Services.AddSingleton<ProfilePage>();
            builder.Services.AddSingleton<DeliveryDetailsPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

    }
}
