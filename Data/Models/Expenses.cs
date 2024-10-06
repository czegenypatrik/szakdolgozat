using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Szakdolgozat.Data.Models
{
    public class Expenses
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Price { get; set; }
        [ForeignKey("User")]
        public string CreatorId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
