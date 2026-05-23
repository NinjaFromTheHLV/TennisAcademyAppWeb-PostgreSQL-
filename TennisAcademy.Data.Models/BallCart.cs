using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TennisAcademyApp.Data.Models
{
    [Comment("Ball Cart")]
    public class BallCart
    {
        [Required]
        [Comment("Foreign Key of Ball")]
        public int BallId { get; set; }
        public virtual Ball Ball { get; set; } = null!;
        [Required]
        [Comment("Foreign Key of IdentityUser")]
        public string UserId { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;
        [Required]
        [Comment("Quantity of Balls in Cart")]
        public int Quantity { get; set; }
        public bool IsOrdered { get; set; } = false;
        public bool IsGift { get; set; } = false;
    }
}
