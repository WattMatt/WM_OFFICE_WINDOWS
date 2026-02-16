using System.ComponentModel.DataAnnotations;

namespace WMOffice.Models
{
    public class StaffMember
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;

        public string? Email { get; set; }

        public string? Phone { get; set; }
    }
}
