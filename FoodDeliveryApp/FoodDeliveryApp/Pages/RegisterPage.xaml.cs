using FoodDeliveryApp.MVVM;
namespace FoodDeliveryApp.Pages;

public partial class RegisterPage : ContentPage
{
    private Button _PasswordEyeButton;
    private Button _ConfirmPasswordEyeButton;
    private Entry _PasswordEntry;
    private Entry _ConfirmPasswordEntry;
    public RegisterPage()
	{
		InitializeComponent();
        PasswordEyeButton = this.FindByName<Button>("PasswordEyeButton");
        ConfirmPasswordEyeButton = this.FindByName<Button>("ConfirmPasswordEyeButton");
        PasswordEntry = this.FindByName<Entry>("PasswordEntry");
        ConfirmPasswordEntry = this.FindByName<Entry>("ConfirmPasswordEntry");
    }
    // Toggle visibility of Old Password
    private void OnPasswordEyeButtonClicked(object sender, EventArgs e)
    {
        // Toggle the IsPassword property on OldPasswordEntry
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;

        // Optionally, change the icon to show different states (open/closed eye)
        PasswordEyeButton.Text = PasswordEntry.IsPassword ? "\uf070" : "\uf06e";
        //OldPasswordEyeButton.FontFamily = "FontAwesome";
    }

    // Toggle visibility of New Password
    private void OnConfirmPasswordEyeButtonClicked(object sender, EventArgs e)
    {
        // Toggle the IsPassword property on NewPasswordEntry
        ConfirmPasswordEntry.IsPassword = !ConfirmPasswordEntry.IsPassword;

        // Optionally, change the icon to show different states (open/closed eye)
        ConfirmPasswordEyeButton.Text = ConfirmPasswordEntry.IsPassword ? "\uf070" : "\uf06e";
        //NewPasswordEyeButton.FontFamily = "FontAwesome";
    }
}