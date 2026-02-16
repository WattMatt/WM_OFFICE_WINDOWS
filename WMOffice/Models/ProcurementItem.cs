using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMOffice.Models
{
    public class ProcurementItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Item { get; set; } = string.Empty;

        [Required]
        public string Supplier { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        [Required]
        public string Status { get; set; } = "Pending"; // Pending, Ordered, Delivered, Cancelled
    }
}
