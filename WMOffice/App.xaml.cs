using Microsoft.UI.Xaml;
using WMOffice.Views;

namespace WMOffice
{
    public partial class App : Application
    {
        private Window m_window;

        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            // Show Login Window first
            m_window = new LoginPage();
            m_window.Activate();
        }

        public void OnLoginSuccess(Window mainWindow)
        {
            // Close the login window and show the main window
            // Note: WinUI 3 Window.Close() disposes the window.
            m_window.Close();
            m_window = mainWindow;
            m_window.Activate();
        }
    }
}
