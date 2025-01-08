using FoodDeliveryApp.MVVM;
namespace FoodDeliveryApp.Pages;
public partial class ModifyPasswordPage : ContentPage
{
    private Button _OldPasswordEyeButton;
    private Button _NewPasswordEyeButton;
    private Entry _OldPasswordEntry;
    private Entry _NewPasswordEntry;
    public ModifyPasswordPage()
	{
		InitializeComponent();
        OldPasswordEyeButton = this.FindByName<Button>("OldPasswordEyeButton");
        NewPasswordEyeButton = this.FindByName<Button>("NewPasswordEyeButton");
        OldPasswordEntry = this.FindByName<Entry>("OldPasswordEntry");
        NewPasswordEntry = this.FindByName<Entry>("NewPasswordEntry");

    }
    // Toggle visibility of Old Password
    private void OnOldPasswordEyeButtonClicked(object sender, EventArgs e)
    {
        // Toggle the IsPassword property on OldPasswordEntry
        OldPasswordEntry.IsPassword = !OldPasswordEntry.IsPassword;

        // Optionally, change the icon to show different states (open/closed eye)
        OldPasswordEyeButton.Text = OldPasswordEntry.IsPassword ? "\uf070" : "\uf06e";
        //OldPasswordEyeButton.FontFamily = "FontAwesome";
    }

    // Toggle visibility of New Password
    private void OnNewPasswordEyeButtonClicked(object sender, EventArgs e)
    {
        // Toggle the IsPassword property on NewPasswordEntry
        NewPasswordEntry.IsPassword = !NewPasswordEntry.IsPassword;

        // Optionally, change the icon to show different states (open/closed eye)
        NewPasswordEyeButton.Text = NewPasswordEntry.IsPassword ? "\uf070" : "\uf06e";
        //NewPasswordEyeButton.FontFamily = "FontAwesome";
    }
}