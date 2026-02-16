using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WMOffice.Models;
// Assuming a namespace for standard WPF commands or using RelayCommand pattern.
// For simplicity, I will mock a RelayCommand implementation inline or assume usage of CommunityToolkit.Mvvm if available.
// Since I don't know the full project context, I'll write a basic RelayCommand implementation inside the file for self-containment.

namespace WMOffice.ViewModels
{
    public class SiteDiaryViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<SiteDiaryEntry> _diaryEntries;
        private SiteDiaryEntry _selectedEntry;

        public SiteDiaryViewModel()
        {
            // Initialize with dummy data or empty collection
            DiaryEntries = new ObservableCollection<SiteDiaryEntry>
            {
                new SiteDiaryEntry { Date = DateTime.Now, WeatherSummary = "Sunny", Notes = "Started foundation work." },
                new SiteDiaryEntry { Date = DateTime.Now.AddDays(-1), WeatherSummary = "Cloudy", Notes = "Excavation completed." }
            };

            AddEntryCommand = new RelayCommand(AddEntry);
            SaveCommand = new RelayCommand(SaveEntry, () => SelectedEntry != null);
            DeleteCommand = new RelayCommand(DeleteEntry, () => SelectedEntry != null);
        }

        public ObservableCollection<SiteDiaryEntry> DiaryEntries
        {
            get => _diaryEntries;
            set
            {
                _diaryEntries = value;
                OnPropertyChanged();
            }
        }

        public SiteDiaryEntry SelectedEntry
        {
            get => _selectedEntry;
            set
            {
                _selectedEntry = value;
                OnPropertyChanged();
                // Update command execution status
                ((RelayCommand)SaveCommand).RaiseCanExecuteChanged();
                ((RelayCommand)DeleteCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand AddEntryCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        private void AddEntry()
        {
            var newEntry = new SiteDiaryEntry
            {
                Date = DateTime.Now,
                WeatherSummary = "New Entry",
                Notes = ""
            };
            DiaryEntries.Add(newEntry);
            SelectedEntry = newEntry;
        }

        private void SaveEntry()
        {
            // In a real app, this would commit to EF Core context
            // context.SaveChanges();
            Console.WriteLine($"Saved entry for {SelectedEntry?.Date}");
        }

        private void DeleteEntry()
        {
            if (SelectedEntry != null)
            {
                DiaryEntries.Remove(SelectedEntry);
                SelectedEntry = null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Basic RelayCommand implementation for standalone functionality
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute();
        }
    }
}
