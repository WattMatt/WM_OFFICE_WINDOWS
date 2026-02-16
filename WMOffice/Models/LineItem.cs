using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMOffice.Models
{
    public class LineItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        public DateTime DateIncurred { get; set; } = DateTime.Now;

        public int CostCategoryId { get; set; }

        [ForeignKey("CostCategoryId")]
        public virtual CostCategory CostCategory { get; set; } = null!;
    }
}
