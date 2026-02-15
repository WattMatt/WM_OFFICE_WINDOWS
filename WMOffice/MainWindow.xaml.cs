using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using WMOffice.Models;
using WMOffice.Services;

namespace WMOffice
{
    public sealed partial class MainWindow : Window
    {
        public ObservableCollection<Project> Projects { get; } = new ObservableCollection<Project>();
        private ProjectService _projectService;

        public MainWindow()
        {
            this.InitializeComponent();
            _projectService = new ProjectService();
            LoadProjects();
            
            // Set initial Title
            Title = "WM Office Dashboard";
        }

        private async void LoadProjects()
        {
            var data = await _projectService.GetProjectsAsync();
            Projects.Clear();
            foreach (var item in data)
            {
                Projects.Add(item);
            }
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                // Navigate to settings
            }
            else
            {
                var selectedItem = (NavigationViewItem)args.SelectedItem;
                // Simple navigation logic for scaffold
                if ((string)selectedItem.Tag == "Dashboard")
                {
                    DashboardPanel.Visibility = Visibility.Visible;
                }
                else
                {
                    // Placeholder for other pages
                    DashboardPanel.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
