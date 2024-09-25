using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Szakdolgozat.Data.Models
{
    public class Membership
    {
        [Key]
        Guid Id { get; set; }
        [Required]
        string Type { get; set; }
        [Required]
        int Price { get; set; }
        string? Description { get; set; }
    }
}
