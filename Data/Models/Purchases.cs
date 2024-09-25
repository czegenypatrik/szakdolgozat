using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Szakdolgozat.Data.Models
{
    public class Purchases
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey(nameof(Members.Id))]
        public Guid BuyerId {  get; set; }
        [ForeignKey(nameof(Membership.Id))]
        public Guid PassId { get; set; }
        public int Price { get; set; }
        [ForeignKey("User")]
        public string CreatorId { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
