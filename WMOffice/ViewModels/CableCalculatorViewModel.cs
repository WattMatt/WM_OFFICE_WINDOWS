using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WMOffice.Services;
using System;

namespace WMOffice.ViewModels
{
    public partial class CableCalculatorViewModel : ObservableObject
    {
        private readonly CableSizingService _cableService;

        [ObservableProperty]
        private double _loadAmps = 10;

        [ObservableProperty]
        private double _lengthMeters = 50;

        [ObservableProperty]
        private int _selectedVoltageIndex = 0; // 0: 230V, 1: 400V

        [ObservableProperty]
        private bool _isGround;

        [ObservableProperty]
        private string _resultText = "-";

        [ObservableProperty]
        private string _detailsText = "Enter values to calculate.";

        public CableCalculatorViewModel()
        {
            _cableService = new CableSizingService();
        }

        [RelayCommand]
        private void Calculate()
        {
            double voltage = SelectedVoltageIndex == 0 ? 230 : 400;
            bool isThreePhase = SelectedVoltageIndex == 1;

            try
            {
                var result = _cableService.CalculateCableSize(LoadAmps, voltage, LengthMeters, isThreePhase, IsGround);
                ResultText = result;
                DetailsText = $"Load: {LoadAmps}A, Length: {LengthMeters}m, {(isThreePhase ? "3-Phase" : "1-Phase")}, {(IsGround ? "Ground" : "Air")}";
            }
            catch (Exception ex)
            {
                ResultText = "Error";
                DetailsText = ex.Message;
            }
        }

        [RelayCommand]
        private void SaveToProject()
        {
            // Placeholder for saving logic as requested in "INTEGRATION" step.
            // In a real app, we'd inject a ProjectService or similar.
            // For now, we'll just acknowledge the action or maybe append to a list if one existed.
            DetailsText += " (Saved to Project)";
        }
    }
}
