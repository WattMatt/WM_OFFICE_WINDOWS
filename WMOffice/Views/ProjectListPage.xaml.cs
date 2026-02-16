using Microsoft.UI.Xaml.Controls;
using WMOffice.ViewModels;

namespace WMOffice.Views
{
    public sealed partial class ProjectListPage : Page
    {
        public ProjectListViewModel ViewModel { get; }

        public ProjectListPage()
        {
            this.InitializeComponent();
            ViewModel = new ProjectListViewModel();
            this.DataContext = ViewModel; // Ensure Binding works against ViewModel
        }
    }
}
