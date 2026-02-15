using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMOffice.Models;

namespace WMOffice.Services
{
    public class ProjectService
    {
        private List<Project> _projects;

        public ProjectService()
        {
            // Mock Data
            _projects = new List<Project>
            {
                new Project("1", "Website Redesign", "active"),
                new Project("2", "Mobile App Launch", "active"),
                new Project("3", "Legacy Migration", "completed"),
                new Project("4", "Cloud Infrastructure", "active")
            };
        }

        public Task<List<Project>> GetProjectsAsync()
        {
            return Task.FromResult(_projects);
        }

        public Task<Project> GetProjectByIdAsync(string id)
        {
            var project = _projects.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(project);
        }
    }
}
