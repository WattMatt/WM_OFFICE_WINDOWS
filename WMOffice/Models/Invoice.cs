using System;
using System.ComponentModel.DataAnnotations;

namespace WMOffice.Models
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string InvoiceNumber { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        [Required]
        public string Status { get; set; } // "Paid", "Unpaid"
    }
}
