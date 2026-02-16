using Microsoft.UI.Xaml.Controls;
using WMOffice.ViewModels;

namespace WMOffice.Views
{
    public sealed partial class CableSchedulePage : Page
    {
        public CableScheduleViewModel ViewModel => (CableScheduleViewModel)DataContext;

        public CableSchedulePage()
        {
            this.InitializeComponent();
        }
    }
}
