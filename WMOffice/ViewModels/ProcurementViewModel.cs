using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using WMOffice.Models;

namespace WMOffice.ViewModels
{
    public class ProcurementViewModel : INotifyPropertyChanged
    {
        private string _searchText;
        private string _selectedStatusFilter = "All";
        private ObservableCollection<ProcurementItem> _items;
        private ICollectionView _filteredItems;

        public ProcurementViewModel()
        {
            // Dummy data for design/testing since no actual DB context is provided yet
            _items = new ObservableCollection<ProcurementItem>
            {
                new ProcurementItem { Id = 1, Item = "Office Chairs (10)", Supplier = "Herman Miller", OrderDate = DateTime.Now.AddDays(-5), Status = "Ordered" },
                new ProcurementItem { Id = 2, Item = "Laptops (5)", Supplier = "Dell", OrderDate = DateTime.Now.AddDays(-2), Status = "Pending" },
                new ProcurementItem { Id = 3, Item = "Paper Reams (50)", Supplier = "Staples", OrderDate = DateTime.Now.AddDays(-10), DeliveryDate = DateTime.Now.AddDays(-1), Status = "Delivered" }
            };

            _filteredItems = CollectionViewSource.GetDefaultView(_items);
            _filteredItems.Filter = FilterProcurementItems;
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged();
                    _filteredItems.Refresh();
                }
            }
        }

        public string SelectedStatusFilter
        {
            get => _selectedStatusFilter;
            set
            {
                if (_selectedStatusFilter != value)
                {
                    _selectedStatusFilter = value;
                    OnPropertyChanged();
                    _filteredItems.Refresh();
                }
            }
        }

        public ICollectionView FilteredItems => _filteredItems;

        private bool FilterProcurementItems(object obj)
        {
            if (obj is ProcurementItem item)
            {
                bool matchesSearch = string.IsNullOrWhiteSpace(_searchText) ||
                                     item.Item.IndexOf(_searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                     item.Supplier.IndexOf(_searchText, StringComparison.OrdinalIgnoreCase) >= 0;

                bool matchesStatus = _selectedStatusFilter == "All" || item.Status == _selectedStatusFilter;

                return matchesSearch && matchesStatus;
            }
            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
