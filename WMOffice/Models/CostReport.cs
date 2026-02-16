using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMOffice.Models
{
    public class CostReport
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [MaxLength(500)]
        public string? Description { get; set; }

        public virtual ObservableCollection<CostCategory> Categories { get; set; } = new ObservableCollection<CostCategory>();

        [NotMapped]
        public decimal TotalCost => Categories?.Sum(c => c.TotalCategoryCost) ?? 0;
    }
}
