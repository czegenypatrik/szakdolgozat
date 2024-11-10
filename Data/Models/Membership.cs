using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Szakdolgozat.Data.Models
{
    public class Membership
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Type { get; set; } = default!;
        [Required]
        public int Price { get; set; }
        public string? Description { get; set; }
        public int? ValidDays { get; set; }
    }
}
