using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WMOffice.Models;
using WMOffice.Data;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

namespace WMOffice.ViewModels
{
    public partial class ProjectListViewModel : ObservableObject
    {
        private readonly AppDbContext _context;
        private readonly SyncQueue _syncQueue;

        [ObservableProperty]
        private ObservableCollection<Project> _projects = new();

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private string _newProjectName;

        public ProjectListViewModel()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated(); // Simplified initialization
            _syncQueue = new SyncQueue();
            
            // Start sync processing (fire and forget for now, ideally managed by a service)
            _ = _syncQueue.StartProcessingAsync();
            
            LoadProjects();
        }

        public async void LoadProjects()
        {
            IsLoading = true;
            try
            {
                var list = await _context.Projects.ToListAsync();
                Projects = new ObservableCollection<Project>(list);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        public async Task AddProjectAsync()
        {
            if (string.IsNullOrWhiteSpace(NewProjectName)) return;

            var newProject = new Project(NewProjectName);
            
            _context.Projects.Add(newProject);
            await _context.SaveChangesAsync();
            
            Projects.Add(newProject);
            
            // Queue sync item
            _syncQueue.Enqueue(new { Action = "AddProject", ProjectId = newProject.Id, Name = newProject.Name });
            
            NewProjectName = string.Empty;
        }

        [RelayCommand]
        public async Task DeleteProjectAsync(Project project)
        {
            if (project == null) return;

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            
            Projects.Remove(project);
             _syncQueue.Enqueue(new { Action = "DeleteProject", ProjectId = project.Id });
        }
    }
}
