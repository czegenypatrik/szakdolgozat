using System.ComponentModel.DataAnnotations;

namespace Szakdolgozat.Data.Models
{
    public class Members
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime? Birthday { get; set; }
        public string? Email { get; set; }
        [Required]
        public DateTime JoinedDate { get; set; }
        public string? Gender { get; set; }
    }
}
