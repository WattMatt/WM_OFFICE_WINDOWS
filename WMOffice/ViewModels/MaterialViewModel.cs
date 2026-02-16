using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using WMOffice.Models;
using System.Collections.Generic;

namespace WMOffice.ViewModels
{
    public class MaterialViewModel : INotifyPropertyChanged
    {
        private string _searchText = string.Empty;
        private ObservableCollection<Material> _materials;
        private ObservableCollection<Material> _filteredMaterials;

        public MaterialViewModel()
        {
            // Dummy data for initialization/demonstration
            // In a real app, this would load from the EF Core DbContext
            _materials = new ObservableCollection<Material>
            {
                new Material { Id = 1, Code = "M001", Description = "Cement 50kg Bag", Unit = "Bag", Rate = 12.50m, Supplier = "BuildCo" },
                new Material { Id = 2, Code = "M002", Description = "Sand (River)", Unit = "Ton", Rate = 45.00m, Supplier = "QuarryKing" },
                new Material { Id = 3, Code = "M003", Description = "Bricks (Red)", Unit = "1000", Rate = 300.00m, Supplier = "BrickWorks" },
                new Material { Id = 4, Code = "M004", Description = "Steel Rod 12mm", Unit = "Kg", Rate = 1.20m, Supplier = "MetalCorp" }
            };

            _filteredMaterials = new ObservableCollection<Material>(_materials);
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
                    FilterMaterials();
                }
            }
        }

        public ObservableCollection<Material> FilteredMaterials
        {
            get => _filteredMaterials;
            set
            {
                _filteredMaterials = value;
                OnPropertyChanged();
            }
        }

        private void FilterMaterials()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredMaterials = new ObservableCollection<Material>(_materials);
            }
            else
            {
                var lowerSearch = SearchText.ToLower();
                var filtered = _materials.Where(m => 
                    m.Code.ToLower().Contains(lowerSearch) || 
                    m.Description.ToLower().Contains(lowerSearch) ||
                    m.Supplier.ToLower().Contains(lowerSearch));
                
                FilteredMaterials = new ObservableCollection<Material>(filtered);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
