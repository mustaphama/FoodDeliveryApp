using CommunityToolkit.Maui.Views;

namespace FoodDeliveryApp.Popups;

public partial class RemoveItemPopup : Popup
{
    public string ItemName { get; set; }
    public Action<bool> OnResult { get; set; }

    public RemoveItemPopup(string itemName, Action<bool> onResult)
    {
        InitializeComponent();
        ItemName = itemName;
        OnResult = onResult;
        BindingContext = this;
    }

    private void OnYesClicked(object sender, EventArgs e)
    {
        OnResult?.Invoke(true);
        Close();
    }

    private void OnNoClicked(object sender, EventArgs e)
    {
        OnResult?.Invoke(false);
        Close();
    }
}
