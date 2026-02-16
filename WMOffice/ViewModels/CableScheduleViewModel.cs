using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WMOffice.Models;
using WMOffice.Services;
using System.Linq;

namespace WMOffice.ViewModels
{
    public partial class CableScheduleViewModel : ObservableObject
    {
        private readonly CableSizingService _sizingService;

        [ObservableProperty]
        private ObservableCollection<CableRun> _cableRuns = new();

        [ObservableProperty]
        private CableRun? _selectedRun;

        public CableScheduleViewModel()
        {
            _sizingService = new CableSizingService();
            // Add some dummy data for preview
            CableRuns.Add(new CableRun { Tag = "C-001", From = "DB-1", To = "M-1", Load = 10, Length = 25 });
            CableRuns.Add(new CableRun { Tag = "C-002", From = "DB-1", To = "L-1", Load = 5, Length = 50 });
        }

        [RelayCommand]
        private void Calculate()
        {
            foreach (var run in CableRuns)
            {
                // Assuming 230V Single Phase, Air installation as default for the schedule unless columns added
                // For a robust app, we'd add properties to CableRun for Phase/InstallType.
                // Using defaults: 230V, 1-Phase, Air (False, False)
                
                // Note: The service signature is (load, voltage, length, isThreePhase, isGround)
                // We'll assume standard 230V 1-phase for now as per simple model.
                
                run.CableSize = _sizingService.CalculateCableSize(run.Load, 230, run.Length);
            }
        }

        [RelayCommand]
        private void AddRun()
        {
            CableRuns.Add(new CableRun { Tag = "New Run" });
        }

        [RelayCommand]
        private void RemoveRun()
        {
            if (SelectedRun != null)
            {
                CableRuns.Remove(SelectedRun);
            }
        }
    }
}
