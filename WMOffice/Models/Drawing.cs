using System;
using System.ComponentModel.DataAnnotations;

namespace WMOffice.Models
{
    public class Drawing
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string DrawingNumber { get; set; }

        [Required]
        public string Title { get; set; }

        public string Revision { get; set; }

        public string Status { get; set; }

        public string ImagePath { get; set; }
    }
}
