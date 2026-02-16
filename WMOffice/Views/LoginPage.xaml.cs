using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WMOffice.ViewModels;

namespace WMOffice.Views
{
    public sealed partial class LoginPage : Window
    {
        public LoginViewModel ViewModel { get; }

        public LoginPage()
        {
            this.InitializeComponent();
            ViewModel = new LoginViewModel(WMOffice.Services.AuthService.Instance);
            
            // In WinUI 3, Window doesn't inherit from FrameworkElement, so no DataContext.
            // We set it on the root content.
            if (this.Content is FrameworkElement root)
            {
                root.DataContext = ViewModel;
            }
            
            this.Title = "Login - WMOffice";
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                ViewModel.Password = passwordBox.Password;
            }
        }
    }
}
