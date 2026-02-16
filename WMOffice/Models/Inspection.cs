using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMOffice.Models
{
    public class Inspection
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        public string Description { get; set; }

        public string InspectorName { get; set; }

        public List<Photo> Photos { get; set; } = new List<Photo>();
    }
}
