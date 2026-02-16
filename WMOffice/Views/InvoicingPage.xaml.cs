using Microsoft.UI.Xaml.Controls;
using WMOffice.ViewModels;

namespace WMOffice.Views
{
    public sealed partial class InvoicingPage : Page
    {
        public InvoicingViewModel ViewModel { get; }

        public InvoicingPage()
        {
            this.InitializeComponent();
            ViewModel = new InvoicingViewModel();
            this.DataContext = ViewModel;
        }
    }
}
