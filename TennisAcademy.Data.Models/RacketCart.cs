using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TennisAcademyApp.Data.Models
{
    [Comment("Racket Cart")]
    public class RacketCart
    {
        [Required]
        [Comment("Foreign Key of Racket")]
        public int RacketId { get; set; }
        public virtual Racket Racket { get; set; } = null!;
        [Required]
        [Comment("Foreign Key of IdentityUser")]
        public string UserId { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;
        [Required]
        [Comment("Quantity of Rackets in Cart")]
        public int Quantity { get; set; }
        public bool IsOrdered { get; set; } = false;
        public bool IsGift { get; set; } = false;
    }
}
