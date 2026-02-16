using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMOffice.Models
{
    public class Photo
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string InspectionId { get; set; }

        [ForeignKey("InspectionId")]
        public Inspection Inspection { get; set; }

        [Required]
        public string FilePath { get; set; }

        public string Note { get; set; }

        // Geolocation data
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        // Normalized coordinates for pin placement on a floor plan/map image
        // 0.0 to 1.0 range
        public double? NormalizedX { get; set; }
        public double? NormalizedY { get; set; }

        public DateTime CapturedAt { get; set; } = DateTime.UtcNow;
    }
}
