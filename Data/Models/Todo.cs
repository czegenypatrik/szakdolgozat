using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Szakdolgozat.Data.Models
{
    public class Todo
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        public DateTime? DueDate { get; set; }
        [ForeignKey("User")]
        public string CreatorId { get; set; }
        public int Severity { get; set; }
        public string Status { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
