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
    public partial class FloorPlanViewModel : ObservableObject
    {
        private readonly AppDbContext _context;

        [ObservableProperty]
        private ObservableCollection<Photo> _photos = new();
        
        [ObservableProperty]
        private Inspection _currentInspection;

        public FloorPlanViewModel()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated();
            
            // For demo purposes, load the first inspection's photos
            LoadInspectionAsync();
        }

        public async void LoadInspectionAsync()
        {
            var inspection = await _context.Inspections.Include(i => i.Photos).FirstOrDefaultAsync();
            if (inspection != null)
            {
                CurrentInspection = inspection;
                Photos = new ObservableCollection<Photo>(inspection.Photos.Where(p => p.NormalizedX.HasValue && p.NormalizedY.HasValue));
            }
        }

        [RelayCommand]
        public async Task AddPinAsync(Photo photo)
        {
            if (CurrentInspection == null) return;
            
            photo.InspectionId = CurrentInspection.Id;
            photo.CapturedAt = System.DateTime.UtcNow;
            photo.FilePath = "ms-appx:///Assets/StoreLogo.png"; // Placeholder
            
            _context.Photos.Add(photo);
            await _context.SaveChangesAsync();
            
            Photos.Add(photo);
        }
    }
}
