﻿using CommunityToolkit.Maui;
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
                });
            // Register CartService as a Singleton
            builder.Services.AddSingleton<CartService>();

            // Register ViewModels with DI
            builder.Services.AddSingleton<ProductDetailsViewModel>();
            builder.Services.AddSingleton<CartViewModel>();

            // Register Pages with DI
            builder.Services.AddSingleton<CartPage>();
            builder.Services.AddSingleton<ProductDetailsPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

    }
}
