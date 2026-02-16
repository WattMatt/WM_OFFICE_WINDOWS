using System;
using System.ComponentModel.DataAnnotations;

namespace WMOffice.Models
{
    public class SiteDiaryEntry
    {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string WeatherSummary { get; set; }

        public string Notes { get; set; }

        // Additional fields implied by "concept" (photos, tasks) could be added here later
        // or as related entities. Keeping it simple for the port step 1.
    }
}
