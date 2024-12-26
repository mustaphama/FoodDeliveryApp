using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace FoodDeliveryApp.MVVM
{
    public partial class ProfileDetailsViewModel : ObservableObject
    {
        // Commands
        public ICommand ModifyNameCommand { get; }
        public ICommand ModifyEmailCommand { get; }
        public ICommand ModifyPasswordCommand { get; }

        public ProfileDetailsViewModel()
        {
            // Initialize commands
            ModifyNameCommand = new AsyncRelayCommand(NavigateToModifyNameAsync);
            ModifyEmailCommand = new AsyncRelayCommand(NavigateToModifyEmailAsync);
            ModifyPasswordCommand = new AsyncRelayCommand(NavigateToModifyPasswordAsync);
        }

        private async Task NavigateToModifyNameAsync()
        {
            await Shell.Current.GoToAsync("ModifyNamePage");
        }

        private async Task NavigateToModifyEmailAsync()
        {
            await Shell.Current.GoToAsync("ModifyEmailPage");
        }

        private async Task NavigateToModifyPasswordAsync()
        {
            Debug.WriteLine("Navigating to Password Page");
            await Shell.Current.GoToAsync("ModifyPasswordPage");
        }
    }
}
