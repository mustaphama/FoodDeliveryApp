using System.Security.Cryptography.X509Certificates;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodDeliveryApp.Models;
using System.Collections.ObjectModel;
namespace FoodDeliveryApp.MVVM;

public partial class PromotionViewModel : ObservableObject
{
    private readonly ApiService _apiService;
    public ObservableCollection<UserCards> PromotionCards { get; } = new ObservableCollection<UserCards>();

    public PromotionViewModel()
    {
        _apiService = new ApiService();
        LoadPromotionCardDataAsync();
    }
    [RelayCommand]
    public async Task LoadPromotionCardDataAsync()
    {
        try
        {
            var userId = Preferences.Get("UserId", 0);

            if (userId == 0)
            {
                await Shell.Current.GoToAsync("MainPage");
                return;
            }
            var promotionCards = await _apiService.GetPromotionCardsByUserId(userId);
            PromotionCards.Clear();

            foreach (var card in promotionCards)
            {
                PromotionCards.Add(card);
            }

        }
        catch (Exception ex)
        {
            // Handle exceptions
            Console.WriteLine($"Error loading restaurant details: {ex.Message}");
        }
    }
}
