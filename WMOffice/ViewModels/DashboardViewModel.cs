using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using Microsoft.EntityFrameworkCore;
using WMOffice.Data;
using WMOffice.Models;

namespace WMOffice.ViewModels
{
    public class DashboardMetric
    {
        public string Title { get; set; }
        public string Value { get; set; }
        public string Icon { get; set; } // E.g., a Segoe MDL2 Assets character
        public Brush Color { get; set; }
    }

    public class BudgetCategory
    {
        public string Name { get; set; }
        public double Spent { get; set; }
        public double Total { get; set; }
        public double Percentage => Total > 0 ? (Spent / Total) * 100 : 0;
        public string DisplayPercentage => $"{Percentage:F1}%";
        public Brush BarColor { get; set; }
    }

    public partial class DashboardViewModel : ObservableObject
    {
        private readonly AppDbContext _context;

        public ObservableCollection<DashboardMetric> KeyMetrics { get; set; }
        public ObservableCollection<BudgetCategory> BudgetOverview { get; set; }
        public ObservableCollection<Project> ActiveProjects { get; set; }

        [ObservableProperty]
        private string _welcomeMessage;

        public DashboardViewModel()
        {
            _context = new AppDbContext();
            KeyMetrics = new ObservableCollection<DashboardMetric>();
            BudgetOverview = new ObservableCollection<BudgetCategory>();
            ActiveProjects = new ObservableCollection<Project>();
            WelcomeMessage = $"Welcome back! {System.DateTime.Now:D}";
            
            // Load data asynchronously
            _ = LoadDashboardDataAsync();
        }

        private async Task LoadDashboardDataAsync()
        {
            try 
            {
                // 1. Key Metrics
                var projectCount = await _context.Projects.CountAsync();
                var activeProjectCount = await _context.Projects.CountAsync(p => p.Status == "Active");
                
                // Mocking some financial data since we might not have a full ledger yet
                var totalInvoiced = 150000.00; 
                var pendingInvoices = 45000.00;

                KeyMetrics.Clear();
                KeyMetrics.Add(new DashboardMetric { Title = "Active Projects", Value = activeProjectCount.ToString(), Icon = "\uE71D", Color = Brushes.DeepSkyBlue });
                KeyMetrics.Add(new DashboardMetric { Title = "Total Projects", Value = projectCount.ToString(), Icon = "\uE8F1", Color = Brushes.Gray });
                KeyMetrics.Add(new DashboardMetric { Title = "Revenue (YTD)", Value = $"${totalInvoiced:N0}", Icon = "\uE155", Color = Brushes.Teal });
                KeyMetrics.Add(new DashboardMetric { Title = "Pending", Value = $"${pendingInvoices:N0}", Icon = "\uE1A5", Color = Brushes.Orange });

                // 2. Budget Overview (Simulating Charts with Progress Bars)
                BudgetOverview.Clear();
                BudgetOverview.Add(new BudgetCategory { Name = "Materials", Spent = 45000, Total = 60000, BarColor = Brushes.CornflowerBlue });
                BudgetOverview.Add(new BudgetCategory { Name = "Labor", Spent = 32000, Total = 40000, BarColor = Brushes.PaleVioletRed });
                BudgetOverview.Add(new BudgetCategory { Name = "Equipment", Spent = 12000, Total = 25000, BarColor = Brushes.MediumSeaGreen });
                BudgetOverview.Add(new BudgetCategory { Name = "Overhead", Spent = 5000, Total = 8000, BarColor = Brushes.Goldenrod });

                // 3. Recent Projects
                var recent = await _context.Projects
                    .OrderByDescending(p => p.CreatedAt)
                    .Take(5)
                    .ToListAsync();

                ActiveProjects.Clear();
                foreach(var p in recent) ActiveProjects.Add(p);
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading dashboard: {ex.Message}");
            }
        }
    }
}
