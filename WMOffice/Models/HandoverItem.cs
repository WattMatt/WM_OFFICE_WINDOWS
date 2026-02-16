using System;
using System.ComponentModel.DataAnnotations;

namespace WMOffice.Models
{
    public class HandoverItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string DocumentName { get; set; } = string.Empty;

        public bool IsRequired { get; set; }

        public string Status { get; set; } = "Pending"; // Pending, Completed, Skipped

        public string? Link { get; set; }
    }
}
