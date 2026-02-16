using Microsoft.UI.Xaml.Controls;
using WMOffice.ViewModels;

namespace WMOffice.Views
{
    public sealed partial class CableCalculatorPage : Page
    {
        public CableCalculatorViewModel ViewModel { get; }

        public CableCalculatorPage()
        {
            this.InitializeComponent();
            ViewModel = new CableCalculatorViewModel();
            this.DataContext = ViewModel;
        }
    }
}
