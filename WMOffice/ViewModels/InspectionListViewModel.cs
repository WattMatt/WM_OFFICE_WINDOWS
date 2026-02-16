using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WMOffice.Models;
using WMOffice.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace WMOffice.ViewModels
{
    public partial class InspectionListViewModel : ObservableObject
    {
        private readonly AppDbContext _context;

        [ObservableProperty]
        private ObservableCollection<Inspection> _inspections = new();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsInspectionSelected))]
        private Inspection _selectedInspection;
        
        public bool IsInspectionSelected => SelectedInspection != null;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private string _newInspectionDescription;
        
        [ObservableProperty]
        private string _newInspectorName;

        public InspectionListViewModel()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated();
            LoadInspections();
        }

        public async void LoadInspections()
        {
            IsLoading = true;
            try
            {
                var list = await _context.Inspections.Include(i => i.Project).ToListAsync();
                Inspections = new ObservableCollection<Inspection>(list);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        public async Task AddInspectionAsync()
        {
            if (string.IsNullOrWhiteSpace(NewInspectionDescription)) return;

            // For simplicity, we just pick the first project or create a default one if none exists
            var project = await _context.Projects.FirstOrDefaultAsync();
            if (project == null)
            {
                project = new Project("Default Project");
                _context.Projects.Add(project);
                await _context.SaveChangesAsync();
            }

            var newInspection = new Inspection
            {
                ProjectId = project.Id,
                Description = NewInspectionDescription,
                InspectorName = NewInspectorName ?? "Unknown",
                Date = System.DateTime.UtcNow
            };
            
            _context.Inspections.Add(newInspection);
            await _context.SaveChangesAsync();
            
            Inspections.Add(newInspection);
            SelectedInspection = newInspection;
            
            NewInspectionDescription = string.Empty;
            NewInspectorName = string.Empty;
        }

        [RelayCommand]
        public async Task DeleteInspectionAsync()
        {
            if (SelectedInspection == null) return;

            _context.Inspections.Remove(SelectedInspection);
            await _context.SaveChangesAsync();
            
            Inspections.Remove(SelectedInspection);
            SelectedInspection = null;
        }
    }
}
