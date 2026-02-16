using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace WMOffice.Models
{
    public class CostCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public int CostReportId { get; set; }

        [ForeignKey("CostReportId")]
        public virtual CostReport CostReport { get; set; } = null!;

        public virtual ObservableCollection<LineItem> LineItems { get; set; } = new ObservableCollection<LineItem>();

        [NotMapped]
        public decimal TotalCategoryCost => LineItems?.Sum(i => i.Amount) ?? 0;
    }
}
