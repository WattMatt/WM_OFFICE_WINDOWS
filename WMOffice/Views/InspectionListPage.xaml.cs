using Microsoft.UI.Xaml.Controls;
using WMOffice.ViewModels;

namespace WMOffice.Views
{
    public sealed partial class InspectionListPage : Page
    {
        public InspectionListPage()
        {
            this.InitializeComponent();
        }

        public InspectionListViewModel ViewModel => (InspectionListViewModel)DataContext;
    }
}
