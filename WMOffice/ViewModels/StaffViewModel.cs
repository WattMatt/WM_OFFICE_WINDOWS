using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WMOffice.Models;

namespace WMOffice.ViewModels
{
    public class StaffViewModel : INotifyPropertyChanged
    {
        private StaffMember _selectedStaffMember;
        private StaffMember _currentStaffMember;

        public ObservableCollection<StaffMember> StaffList { get; set; }

        public StaffMember SelectedStaffMember
        {
            get => _selectedStaffMember;
            set
            {
                _selectedStaffMember = value;
                // Clone for editing to avoid live updates in list before save
                if (value != null)
                {
                    CurrentStaffMember = new StaffMember
                    {
                        Id = value.Id,
                        Name = value.Name,
                        Role = value.Role,
                        Email = value.Email,
                        Phone = value.Phone
                    };
                }
                OnPropertyChanged();
            }
        }

        public StaffMember CurrentStaffMember
        {
            get => _currentStaffMember;
            set
            {
                _currentStaffMember = value;
                OnPropertyChanged();
            }
        }

        public ICommand NewCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        public StaffViewModel()
        {
            StaffList = new ObservableCollection<StaffMember>();
            CurrentStaffMember = new StaffMember(); // Initialize blank

            // Mock Data for UI testing
            StaffList.Add(new StaffMember { Id = 1, Name = "Alice Johnson", Role = "Manager", Email = "alice@wmoffice.com", Phone = "555-0101" });
            StaffList.Add(new StaffMember { Id = 2, Name = "Bob Smith", Role = "Developer", Email = "bob@wmoffice.com", Phone = "555-0102" });

            NewCommand = new RelayCommand(NewStaff);
            SaveCommand = new RelayCommand(SaveStaff);
            DeleteCommand = new RelayCommand(DeleteStaff);
        }

        private void NewStaff(object? parameter)
        {
            SelectedStaffMember = null!;
            CurrentStaffMember = new StaffMember();
        }

        private void SaveStaff(object? parameter)
        {
            if (CurrentStaffMember == null) return;

            // Simple mock save logic
            var existing = StaffList.FirstOrDefault(s => s.Id == CurrentStaffMember.Id);
            if (existing != null)
            {
                // Update
                existing.Name = CurrentStaffMember.Name;
                existing.Role = CurrentStaffMember.Role;
                existing.Email = CurrentStaffMember.Email;
                existing.Phone = CurrentStaffMember.Phone;
            }
            else
            {
                // Add
                CurrentStaffMember.Id = StaffList.Count > 0 ? StaffList.Max(s => s.Id) + 1 : 1;
                StaffList.Add(CurrentStaffMember);
            }
            
            // Reset
            NewStaff(null);
        }

        private void DeleteStaff(object? parameter)
        {
            if (SelectedStaffMember != null)
            {
                StaffList.Remove(SelectedStaffMember);
                NewStaff(null);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
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
