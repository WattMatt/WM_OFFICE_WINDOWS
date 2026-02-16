using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WMOffice.Models;
using System.Linq;

namespace WMOffice.ViewModels
{
    public class CostReportViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<CostReport> Reports { get; set; } = new ObservableCollection<CostReport>();

        private CostReport? _selectedReport;
        public CostReport? SelectedReport
        {
            get => _selectedReport;
            set
            {
                _selectedReport = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddReportCommand { get; }
        public ICommand AddCategoryCommand { get; }
        public ICommand AddLineItemCommand { get; }

        public CostReportViewModel()
        {
            // Initialize with dummy data for demonstration
            LoadData();

            AddReportCommand = new RelayCommand(_ => AddNewReport());
            AddCategoryCommand = new RelayCommand(_ => AddNewCategory(), _ => SelectedReport != null);
            AddLineItemCommand = new RelayCommand(param => AddNewLineItem(param as CostCategory), param => param is CostCategory);
        }

        private void LoadData()
        {
            // In a real app, fetch from EF Core context
            var report = new CostReport { Title = "Q1 Expenses", Description = "Office supplies & travel" };
            var cat1 = new CostCategory { Name = "Travel", CostReport = report };
            cat1.LineItems.Add(new LineItem { Description = "Flight to NY", Amount = 450.00m, CostCategory = cat1 });
            cat1.LineItems.Add(new LineItem { Description = "Hotel", Amount = 300.00m, CostCategory = cat1 });

            var cat2 = new CostCategory { Name = "Supplies", CostReport = report };
            cat2.LineItems.Add(new LineItem { Description = "Paper", Amount = 45.00m, CostCategory = cat2 });

            report.Categories.Add(cat1);
            report.Categories.Add(cat2);

            Reports.Add(report);
            SelectedReport = report;
        }

        private void AddNewReport()
        {
            var newReport = new CostReport { Title = "New Report" };
            Reports.Add(newReport);
            SelectedReport = newReport;
        }

        private void AddNewCategory()
        {
            if (SelectedReport == null) return;
            var newCat = new CostCategory { Name = "New Category", CostReport = SelectedReport };
            SelectedReport.Categories.Add(newCat);
            // Notify UI update if necessary (simplified for brevity)
        }

        private void AddNewLineItem(CostCategory? category)
        {
            if (category == null) return;
            var newItem = new LineItem { Description = "New Item", Amount = 0, CostCategory = category };
            category.LineItems.Add(newItem);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    // Simple RelayCommand implementation
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Predicate<object?>? _canExecute;

        public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => _canExecute == null || _canExecute(parameter);
        public void Execute(object? parameter) => _execute(parameter);
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
