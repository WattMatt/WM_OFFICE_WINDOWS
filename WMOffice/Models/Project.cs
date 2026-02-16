using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMOffice.Models
{
    public class Project
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Name { get; set; }

        public string Status { get; set; } = "Active"; // "Active", "Completed", "Archived"

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // EF Core requires a parameterless constructor
        public Project() { }

        public Project(string name)
        {
            Name = name;
        }
    }
}
