using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using WMOffice.Models;

namespace WMOffice.ViewModels
{
    public partial class HandoverViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<HandoverItem> items;

        public HandoverViewModel()
        {
            Items = new ObservableCollection<HandoverItem>
            {
                new HandoverItem { DocumentName = "Architecture Diagram", IsRequired = true, Status = "Pending" },
                new HandoverItem { DocumentName = "API Documentation", IsRequired = true, Status = "Completed", Link = "http://wiki/api" },
                new HandoverItem { DocumentName = "User Manual", IsRequired = false, Status = "Pending" }
            };
        }
    }
}
